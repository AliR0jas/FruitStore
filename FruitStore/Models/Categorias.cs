using System;
using System.Collections.Generic;

namespace FruitStore.Models
{
    public partial class Categorias
    {
        public Categorias()
        {
            Productos = new HashSet<Productos>();
        }

        public int Id { get; set; }
        public string Nombre { get; set; }
        public int Eliminado { get; set; }

        public virtual ICollection<Productos> Productos { get; set; }
    }
}
