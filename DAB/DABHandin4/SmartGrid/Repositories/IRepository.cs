using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using SmartGrid.Models;

namespace SmartGrid.Repositories
{
    public interface IRepository<T> where T : Entity
    {
        void Create(T t);
        T Read(int id);
        DbSet<T> ReadAll();
        void Delete(T t);

    }
}