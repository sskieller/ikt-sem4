using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartGrid.Models;
using SmartGrid.Data;

namespace SmartGrid.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        private SmartGridContext _context;
        public Repository(SmartGridContext context)
        {
            _context = context;
        }

        public void Create(T t)
        {
            _context.Set<T>().Add(t);
        }

        public T Read(int id)
        {
            return _context.Set<T>().Find(id);
        }

        public DbSet<T> ReadAll()
        {
            return _context.Set<T>();
        }


        public void Delete(T t)
        {
            _context.Set<T>().Remove(t);
        }
    }
}