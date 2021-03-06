﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models;

namespace FruitStore.Services
{
    public class CategoriasService
    {
        public List<Categorias> Categorias { get; set; }
        public CategoriasService()
        {
            using (FruteriashopContext context = new FruteriashopContext())
            {
                Repositories.Repository<Categorias> repos = new Repositories.Repository<Categorias>(context);
                Categorias = repos.GetAll().OrderBy(x => x.Nombre).ToList();
            }
        }
    }
}
