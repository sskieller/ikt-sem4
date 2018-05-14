using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartGrid.Models;

namespace SmartGrid.Repositories
{
    public class UnitOfWork<T> where T : Entity
    {
        private readonly SmartGridContext _context;
        public IRepository<T> Repository { get; }


        public UnitOfWork(SmartGridContext context)
        {
            _context = context;
            Repository = new Repository<T>(context);
        }

        public int Commit()
        {
            return _context.SaveChanges();
        }
    }
}