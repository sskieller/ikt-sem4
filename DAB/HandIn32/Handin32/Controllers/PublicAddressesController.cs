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
using Handin32.Data;
using Microsoft.Ajax.Utilities;

namespace Handin32.Controllers
{
    public class PublicAddressesController : ApiController
    {
	    public class StringHelper
	    {
		    public List<string> helper;
		    public StringHelper()
		    {

		    }

		    public string[] returnString()
		    {
			    return helper.ToArray();
		    }
	    }
        private AddressModel db = new AddressModel();

        // GET: api/PublicAddresses
        public IQueryable<PublicAddressDetailDto> GetPublicAddresses()
        {
	        var uow = new UnitOfWork<PublicAddresses>(db);
	        var addr = from p in uow.Repository.ReadAll()
			        .Include(p => p.People)
		        select new PublicAddressDetailDto
		        {
			        Id = p.Id,
			        AddressType = p.AddressType,
			        City = p.City,
			        HouseNumber = p.HouseNumber,
			        StreetName = p.StreetName,
			        ZipCode = p.ZipCode,
		        };
		       

	        return addr;
        }

        // GET: api/PublicAddresses/5
        [ResponseType(typeof(PublicAddressDetailDto))]
        public async Task<IHttpActionResult> GetPublicAddresses(int id)
        {
	        var uow = new UnitOfWork<PublicAddresses>(db);
	        PublicAddresses addr = uow.Repository.Read(id);
	        if (addr == null)
	        {
		        return NotFound();
	        }

	        var addr2 = uow.Repository.ReadAll()
			        .Include(p => p.People)
		        .Select(p =>  new PublicAddressDetailDto()
		        {
			        Id = p.Id,
			        AddressType = p.AddressType,
			        City = p.City,
			        HouseNumber = p.HouseNumber,
			        StreetName = p.StreetName,
			        ZipCode = p.ZipCode
		        }).SingleOrDefault(p => p.Id == id);

	        return Ok(addr2);

		}

        // PUT: api/PublicAddresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPublicAddresses(int id, PublicAddresses publicAddresses)
        {
			/*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != publicAddresses.Id)
            {
                return BadRequest();
            }

            db.Entry(publicAddresses).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PublicAddressesExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
			*/
			return StatusCode(HttpStatusCode.NoContent);
			
        }

        // POST: api/PublicAddresses
        [ResponseType(typeof(PublicAddressDetailDto))]
        public async Task<IHttpActionResult> PostPublicAddresses(PublicAddresses publicAddresses)
        {
	        var uow = new UnitOfWork<PublicAddresses>(db);
	        if (!ModelState.IsValid)
	        {
		        return BadRequest(ModelState);
	        }

	        uow.Repository.Create(publicAddresses);
	        uow.Commit();

	        PublicAddressDetailDto dto = uow.Repository.ReadAll()
		        .Include(p => p.People)
		        .Select(p => new PublicAddressDetailDto
	        {
		        Id = p.Id,
		        AddressType = p.AddressType,
		        City = p.City,
		        HouseNumber = p.HouseNumber,
		        StreetName = p.StreetName,
		        ZipCode = p.ZipCode
	        }).SingleOrDefault(p => p.Id == publicAddresses.Id);


	        return CreatedAtRoute("DefaultApi", new { id = publicAddresses.Id }, dto);
		}


        // DELETE: api/PublicAddresses/5
        [ResponseType(typeof(PublicAddresses))]
        public async Task<IHttpActionResult> DeletePublicAddresses(int id)
        {
			var uow = new UnitOfWork<PublicAddresses>(db);
	        var addr = uow.Repository.Read(id);
	        if (addr == null)
	        {
		        return NotFound();
	        }

	        uow.Repository.Delete(addr);
	        uow.Commit();

	        return Ok(addr);
		}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PublicAddressesExists(int id)
        {
            return db.PublicAddresses.Count(e => e.Id == id) > 0;
        }
    }
}