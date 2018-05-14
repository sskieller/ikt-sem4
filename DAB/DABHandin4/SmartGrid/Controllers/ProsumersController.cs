using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using SmartGrid.Models;
using SmartGrid.Repositories;

namespace SmartGrid.Controllers
{
    public class ProsumersController : ApiController
    {
        private SmartGridContext db = new SmartGridContext();

        private static event EventHandler ProsumerUpdatedEvent;

        static ProsumersController()
        {
            ProsumerUpdatedEvent += EventNotifier.Instance.ProsumersUpdatedEventHandler;
        }

        // GET: api/Prosumers
        public IEnumerable<ProsumerDTO> GetProsumers()
        {
            var uow = new UnitOfWork<Prosumer>(db);

            var Dtos = new List<ProsumerDTO>();

            foreach(var pro in uow.Repository.ReadAll())
                Dtos.Add(new ProsumerDTO(pro));

            return Dtos;
        }

        // GET: api/Prosumers/5
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> GetProsumer(string id)
        {
            Prosumer prosumer = await db.Prosumers.FindAsync(id);
            if (prosumer == null)
            {
                return NotFound();
            }

            return Ok(prosumer);
        }

        // PUT: api/Prosumers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutProsumer(string id, Prosumer prosumer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != prosumer.Name)
            {
                return BadRequest();
            }

            db.Entry(prosumer).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProsumerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Prosumers
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> PostProsumer(ProsumerDTO[] prosumer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var uow = new UnitOfWork<Prosumer>(db);

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
                        PreferedBuyer = (from p in allProsumers where p.Name == pro.PreferedBuyer select p).FirstOrDefault(),
                        Difference = pro.Produced - pro.Consumed
                    });
                    continue;
                }

                oldProsumer.Consumed = pro.Consumed;
                oldProsumer.Produced = pro.Produced;
                oldProsumer.Name = pro.Name;
                oldProsumer.PreferedBuyer =
                    (from p in allProsumers where p.Name == pro.PreferedBuyer select p).FirstOrDefault();
                oldProsumer.Difference = pro.Produced - pro.Consumed;

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

            ProsumerUpdatedEvent?.Invoke(new object(), new EventArgs());

            //return CreatedAtRoute("DefaultApi", new { id = prosumer.Name }, prosumer);
            return Ok();
        }

        // DELETE: api/Prosumers/5
        [ResponseType(typeof(Prosumer))]
        public async Task<IHttpActionResult> DeleteProsumer(string id)
        {
            Prosumer prosumer = await db.Prosumers.FindAsync(id);
            if (prosumer == null)
            {
                return NotFound();
            }

            db.Prosumers.Remove(prosumer);
            await db.SaveChangesAsync();

            return Ok(prosumer);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProsumerExists(string id)
        {
            return db.Prosumers.Count(e => e.Name == id) > 0;
        }
    }
}