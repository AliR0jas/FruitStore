using FruitStore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FruitStore.Repositories
{
    public class ProductosRepository:Repository<Productos>
    {
        public ProductosRepository(FruteriashopContext context) : base(context) { }
        public IEnumerable<Productos> GetProductosByCategoria(string nombre)
        {
            return Context.Productos.Where(x => x.IdCategoriaNavigation.Nombre == nombre);
        }
        public IEnumerable<Productos> GetProductosByCategoria(int? idCategoria)
        {
            return Context.Productos.Include(x=>x.IdCategoriaNavigation).Where(x => idCategoria == null || x.IdCategoria == idCategoria).OrderBy(x => x.Nombre);
        }

        public Productos GetProductosByCategoriaNombre(string categoria, string nombre)
        {
            return Context.Productos.Include(x=>x.IdCategoriaNavigation).FirstOrDefault(x => x.IdCategoriaNavigation.Nombre == categoria && x.Nombre == nombre);
        }
        public override bool Validate(Productos entidad)
        {
            if (entidad.Precio==null || entidad.Precio<=0)
            {
                throw new Exception("Debe indicar el precio del producto a agregar.");
            }
            if (string.IsNullOrWhiteSpace(entidad.UnidadMedida))
            {
                throw new Exception("Debe inidicar la unidad de medida.");
            }
            if (string.IsNullOrWhiteSpace(entidad.Descripcion))
            {
                throw new Exception("Debe indicar la despcrición del producto.");
            }
            if (string.IsNullOrWhiteSpace(entidad.Nombre))
            {
                throw new Exception("Debe indicar el nombre del producto.");
            }
            if (Context.Productos.Any(x => x.Nombre == entidad.Nombre && x.Id!=entidad.Id))
            {
                throw new Exception("Ya existe un producto registrado con el mismo nombre.");
            }

            if (!Context.Categorias.Any(x => x.Id == entidad.IdCategoria && x.Eliminado == 0))
            {
                throw new Exception("No exise la categoria especificada.");
            }
            return true;
        }
            
    }
}
