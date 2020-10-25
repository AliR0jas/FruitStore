using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FruitStore.Models;
using Microsoft.AspNetCore.SignalR;

namespace FruitStore.Repositories
{
    public class Repository<T> where T : class
    {
        public FruteriashopContext Context { get; set; }

        public Repository(FruteriashopContext context)
        {
            Context = context;
        }

        public virtual IEnumerable<T> GetAll()
        {
            return Context.Set<T>();
        }

        public virtual T Get(object id)
        {
            return Context.Find<T>(id);
        }
        public virtual void Insert(T entidad)
        {
            if (Validate(entidad))
            {
                Context.Add<T>(entidad);
                Context.SaveChanges();
            }
        }
        public virtual void Update(T entidad)
        {
            if (Validate(entidad))
            {
                Context.Update<T>(entidad);
                Context.SaveChanges();
            }
        }
        public virtual void Delete(T entidad)
        {
            Context.Remove<T>(entidad);
            Context.SaveChanges();
        }
        public virtual bool Validate(T entidad)
        {
            return true;
        }
    }
}
