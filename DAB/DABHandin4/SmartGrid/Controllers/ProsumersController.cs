using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Http;
using System.Web.Http.Description;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid.Controllers
{
    public class ProsumersController : ApiController
    {
        private readonly SmartGridContext _db = new SmartGridContext();


        // GET: api/Prosumers
        public IEnumerable<ProsumerDTO> GetProsumers()
        {
            var uow = new UnitOfWork<Prosumer>(_db);

            var Dtos = new List<ProsumerDTO>();

            foreach(var pro in uow.Repository.ReadAll())
                Dtos.Add(new ProsumerDTO(pro));

            return Dtos;
        }

        // GET: api/Prosumers/5
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> GetProsumer(string id)
        {
            Prosumer prosumer = await _db.Prosumers.FindAsync(id);
            if (prosumer == null)
            {
                return NotFound();
            }

            return Ok(prosumer);
        }


        // POST: api/Prosumers
        // POST: api/Prosumers
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> PostProsumer(ProsumerDTO[] prosumer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uow = new UnitOfWork<Prosumer>(_db);

            var allProsumers = uow.Repository.ReadAll();

            foreach (var pro in prosumer)
            {
                var oldProsumer =
                    (from op in allProsumers
                        where op.Name.Equals(pro.Name, StringComparison.CurrentCultureIgnoreCase)
                        select op).FirstOrDefault();

                if (oldProsumer == null)
                {
                    uow.Repository.Create(new Prosumer()
                    {
                        Consumed = pro.Consumed,
                        Produced = pro.Produced,
                        Name = pro.Name,
                        PreferedBuyerName = pro.PreferedBuyer,
                        Type = pro.Type,
                        Difference = pro.Produced - pro.Consumed,
                        Remainder = pro.Produced - pro.Consumed
                    });
                    continue;
                }

                oldProsumer.Consumed = pro.Consumed;
                oldProsumer.Produced = pro.Produced;
                oldProsumer.Name = pro.Name;
                oldProsumer.PreferedBuyerName =
                    pro.PreferedBuyer; //allProsumers.Find(pro.PreferedBuyer) != null ? pro.PreferedBuyer : string.Empty;
                oldProsumer.Type = pro.Type;
                oldProsumer.Difference = pro.Produced - pro.Consumed;
                oldProsumer.Remainder = pro.Produced - pro.Consumed;

                uow.Repository.Update(string.Empty, oldProsumer);
            }

            try
            {
                uow.Commit();
            }
            catch (DbUpdateException)
            {
                return BadRequest("You don goofed");
            }

            await PowerDistributer.HandleTransactions();

            //return CreatedAtRoute("DefaultApi", new { id = prosumer.Name }, prosumer);
            return Ok();
        }

        

        internal void UpdateGrids(ProsumerDTO[] prosumer)
        {
            var uow = new UnitOfWork<Prosumer>(_db);

            var allProsumers = uow.Repository.ReadAll();

            foreach (var pro in prosumer)
            {
                var oldProsumer = (from op in allProsumers where op.Name.Equals(pro.Name, StringComparison.CurrentCultureIgnoreCase) select op).FirstOrDefault();

                if (oldProsumer == null)
                {
                    uow.Repository.Create(new Prosumer()
                    {
                        Consumed = pro.Consumed,
                        Produced = pro.Produced,
                        Name = pro.Name,
                        PreferedBuyerName = pro.PreferedBuyer,
                        Type = pro.Type,
                        Difference = pro.Produced - pro.Consumed,
                        Remainder = pro.Produced - pro.Consumed
                    });
                    continue;
                }

                oldProsumer.Consumed = pro.Consumed;
                oldProsumer.Produced = pro.Produced;
                oldProsumer.Name = pro.Name;
                oldProsumer.PreferedBuyerName = pro.PreferedBuyer;//allProsumers.Find(pro.PreferedBuyer) != null ? pro.PreferedBuyer : string.Empty;
                oldProsumer.Type = pro.Type;
                oldProsumer.Difference = pro.Produced - pro.Consumed;
                oldProsumer.Remainder = pro.Produced - pro.Consumed;

                uow.Repository.Update(string.Empty, oldProsumer);
            }


                uow.Commit();
 
        }
        
        // DELETE: api/Prosumers/5
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> DeleteProsumer(string id)
        {
            Prosumer prosumer = await _db.Prosumers.FindAsync(id);
            if (prosumer == null)
            {
                return NotFound();
            }

            _db.Prosumers.Remove(prosumer);
            await _db.SaveChangesAsync();

            return Ok(prosumer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProsumerExists(string id)
        {
            return _db.Prosumers.Count(e => e.Name == id) > 0;
        }
    }
}