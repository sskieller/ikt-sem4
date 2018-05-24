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
using SmartGrid.Repositories;

namespace SmartGrid.Controllers
{
    public class SmartGridModelsController : ApiController
    {
        private readonly SmartGridContext _db = new SmartGridContext();

        // GET: api/SmartGridModels
        public IEnumerable<SmartGridModelDTO> GetSmartGridModels()
        {
            var uow = new UnitOfWork<SmartGridModel>(_db);

            var dtos = new List<SmartGridModelDTO>();

            foreach (var smartGridModel in uow.Repository.ReadAll())
            {
                dtos.Add(new SmartGridModelDTO(smartGridModel));
            }

            return dtos;
        }

        // GET: api/SmartGridModels/5
        [ResponseType(typeof(SmartGridModel))]
        public IHttpActionResult GetSmartGridModel(int id)
        {
            var uow = new UnitOfWork<SmartGridModel>(_db);

            SmartGridModel smartGridModel = uow.Repository.Read(id);
            if (smartGridModel == null)
            {
                return NotFound();
            }

            return Ok(smartGridModel);
        }

        // POST: api/SmartGridModels
        [ResponseType(typeof(SmartGridModel))]
        public IHttpActionResult PostSmartGridModel(SmartGridModel smartGridModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var uow = new UnitOfWork<SmartGridModel>(_db);
            
            //smartGridModel.TimeStamp = DateTime.Now;

            uow.Repository.Create(smartGridModel);
            uow.Commit();
            
            return CreatedAtRoute("DefaultApi", new { id = smartGridModel.SmartGridId }, smartGridModel);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SmartGridModelExists(int id)
        {
            return _db.SmartGridModels.Count(e => e.SmartGridId == id) > 0;
        }
    }
}