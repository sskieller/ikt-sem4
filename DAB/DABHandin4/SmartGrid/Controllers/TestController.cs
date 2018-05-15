using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid.Controllers
{
    public class TestController : ApiController
    {
        private SmartGridContext db = new SmartGridContext();

        // GET: api/Prosumers
        public async Task<IEnumerable<ProsumerDTO>> GetProsumers()
        {
            var uow = new UnitOfWork<Prosumer>(db);

            var Dtos = new List<ProsumerDTO>();

            foreach (var pro in uow.Repository.ReadAll())
                Dtos.Add(new ProsumerDTO(pro));

            await PowerDistributer.HandleTransactions();

            return Dtos;
        }
    }
}
