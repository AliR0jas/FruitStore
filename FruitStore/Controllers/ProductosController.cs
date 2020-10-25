using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using FruitStore.Repositories;
using FruitStore.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System.IO;
using Pomelo.EntityFrameworkCore.MySql.Storage.Internal;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FruitStore.Controllers
{
    public class ProductosController : Controller
    {
        //Inyeccion en controladores
        public IWebHostEnvironment Environment { get; set; }
        public ProductosController(IWebHostEnvironment env)
        {
            Environment = env;
        }
        public IActionResult Index()
        {
            ProductosIndexViewModel vm = new ProductosIndexViewModel();

            FruteriashopContext context = new FruteriashopContext();
            CategoriasRepository categoriasRepository = new CategoriasRepository(context);
            ProductosRepository productosRepository = new ProductosRepository(context);

            int? id = null;
            vm.Categorias = categoriasRepository.GetAll();
            vm.Productos = productosRepository.GetProductosByCategoria(id);
            return View(vm);
        }
        [HttpPost]
        public IActionResult Index(ProductosIndexViewModel vm)
        {
            FruteriashopContext context = new FruteriashopContext();
            CategoriasRepository categoriasRepository = new CategoriasRepository(context);
            ProductosRepository productosRepository = new ProductosRepository(context);

            vm.Categorias = categoriasRepository.GetAll();
            vm.Productos = productosRepository.GetProductosByCategoria(vm.idCategoria);
            return View(vm);
        }
        public IActionResult Agregar()
        {
            ProductosViewModel vm = new ProductosViewModel();
            FruteriashopContext context = new FruteriashopContext();
            CategoriasRepository categoriasRepository = new CategoriasRepository(context);

            vm.Categorias = categoriasRepository.GetAll();
            return View(vm);
        }
        [HttpPost]
        public IActionResult Agregar(ProductosViewModel vm)
        {
            FruteriashopContext context = new FruteriashopContext();
            if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
            {
                ModelState.AddModelError("", "Debes seleccionar un archivo jpeg de menos de 2MB.");
                CategoriasRepository categoriasRepository = new CategoriasRepository(context);
                vm.Categorias = categoriasRepository.GetAll();
                return View(vm);
            }

            try
            {
                ProductosRepository repos = new ProductosRepository(context);
                repos.Insert(vm.Producto);

                //Guardar

                FileStream fs = new FileStream(Environment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                vm.Archivo.CopyTo(fs);
                fs.Close();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository categoriasRepository = new CategoriasRepository(context);
                vm.Categorias = categoriasRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult Editar(int id)
        {
            FruteriashopContext context = new FruteriashopContext();
            ProductosViewModel vm = new ProductosViewModel();

            ProductosRepository pr = new ProductosRepository(context);

            vm.Producto = pr.Get(id);

            if (vm.Producto == null)
            {
                return RedirectToAction("Index");
            }

            CategoriasRepository cr = new CategoriasRepository(context);
            vm.Categorias = cr.GetAll();

            if (System.IO.File.Exists(Environment.WebRootPath + $"/img_frutas/{vm.Producto.Id}.jpg"))
            {
                vm.Imagen = vm.Producto.Id + ".jpg";
            }
            else
            {
                vm.Imagen = "no-disponible.png";
            }

            return View(vm);
        }
        [HttpPost]
        public IActionResult Editar(ProductosViewModel vm)
        {
            FruteriashopContext context = new FruteriashopContext();

            if (vm.Archivo != null)
            {
                if (vm.Archivo.ContentType != "image/jpeg" || vm.Archivo.Length > 1024 * 1024 * 2)
                {
                    ModelState.AddModelError("", "Debes seleccionar un archivo jpeg de menos de 2MB.");
                    CategoriasRepository categoriasRepository = new CategoriasRepository(context);
                    vm.Categorias = categoriasRepository.GetAll();
                    return View(vm);
                }
            }

            try
            {
                ProductosRepository repos = new ProductosRepository(context);

                var p = repos.Get(vm.Producto.Id);

                if (p != null)
                {
                    p.Nombre = vm.Producto.Nombre;
                    p.IdCategoria = vm.Producto.IdCategoria;
                    p.Precio = vm.Producto.Precio;
                    p.Descripcion = vm.Producto.Descripcion;
                    p.UnidadMedida = vm.Producto.UnidadMedida;
                    repos.Update(p);

                    //Guardar
                    if (vm.Archivo != null)
                    {
                        FileStream fs = new FileStream(Environment.WebRootPath + "/img_frutas/" + vm.Producto.Id + ".jpg", FileMode.Create);
                        vm.Archivo.CopyTo(fs);
                        fs.Close();
                    }
                }

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                CategoriasRepository categoriasRepository = new CategoriasRepository(context);
                vm.Categorias = categoriasRepository.GetAll();
                return View(vm);
            }
        }

        public IActionResult Eliminar(int id)
        {
            using (FruteriashopContext context = new FruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var p = repos.Get(id);
                if (p != null)
                {
                    return View(p);
                }
                else
                    return RedirectToAction("Index");
            }
        }
        [HttpPost]
        public IActionResult Eliminar(Productos p)
        {
            using (FruteriashopContext context = new FruteriashopContext())
            {
                ProductosRepository repos = new ProductosRepository(context);
                var producto = repos.Get(p.Id);
                if (producto != null)
                {
                    repos.Delete(producto);
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "El producto no existe, o ya ha sido eliminado.");
                    return View(p);
                }
            }
        }
    }

}