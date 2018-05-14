using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using SmartGrid.Models;

namespace SmartGrid.Controllers
{
    public class SmartGridModelsController : ApiController
    {
        private SmartGridContext db = new SmartGridContext();

        // GET: api/SmartGridModels
        public IQueryable<SmartGridModel> GetSmartGridModels()
        {
            return db.SmartGridModels;
        }

        // GET: api/SmartGridModels/5
        [ResponseType(typeof(SmartGridModel))]
        public IHttpActionResult GetSmartGridModel(int id)
        {
            SmartGridModel smartGridModel = db.SmartGridModels.Find(id);
            if (smartGridModel == null)
            {
                return NotFound();
            }

            return Ok(smartGridModel);
        }

        // PUT: api/SmartGridModels/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSmartGridModel(int id, SmartGridModel smartGridModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != smartGridModel.SmartGridId)
            {
                return BadRequest();
            }

            db.Entry(smartGridModel).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SmartGridModelExists(id))
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

        // POST: api/SmartGridModels
        [ResponseType(typeof(SmartGridModel))]
        public IHttpActionResult PostSmartGridModel(SmartGridModel smartGridModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.SmartGridModels.Add(smartGridModel);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = smartGridModel.SmartGridId }, smartGridModel);
        }

        // DELETE: api/SmartGridModels/5
        [ResponseType(typeof(SmartGridModel))]
        public IHttpActionResult DeleteSmartGridModel(int id)
        {
            SmartGridModel smartGridModel = db.SmartGridModels.Find(id);
            if (smartGridModel == null)
            {
                return NotFound();
            }

            db.SmartGridModels.Remove(smartGridModel);
            db.SaveChanges();

            return Ok(smartGridModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SmartGridModelExists(int id)
        {
            return db.SmartGridModels.Count(e => e.SmartGridId == id) > 0;
        }
    }
}