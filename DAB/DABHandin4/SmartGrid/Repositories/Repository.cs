using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartGrid.Models;

namespace SmartGrid.Repositories
{
    public class Repository<T> : IRepository<T> where T : Entity
    {
        public Repository<T>()
        {

        }
    }
}