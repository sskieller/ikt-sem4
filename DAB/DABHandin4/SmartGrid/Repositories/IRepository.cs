using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SmartGrid.Models;

namespace SmartGrid.Repositories
{
    public interface IRepository<T> where T : Entity
    {
    }
}