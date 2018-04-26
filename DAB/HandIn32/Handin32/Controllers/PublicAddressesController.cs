using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Drawing.Printing;
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
                    People = (from ppl in p.People
                             select ppl.LastName + ", " + ppl.FirstName)
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
			        ZipCode = p.ZipCode,
		            People = (from ppl in p.People
		                select ppl.LastName + ", " + ppl.FirstName)
                }).SingleOrDefault(p => p.Id == id);

	        return Ok(addr2);

		}

        // PUT: api/PublicAddresses/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPublicAddresses(int id, PublicAddresses publicAddresses)
        {

	        var uow = new UnitOfWork<PublicAddresses>(db);

	        var addr = uow.Repository.Read(id);

			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != publicAddresses.Id || addr == null)
            {
                return BadRequest();
            }


	        if (publicAddresses.People != null)
	        {
				//Persons are to be added to the public address
				addr.People.Clear();

				var repo = new UnitOfWork<People>(db);

		        foreach (var person in publicAddresses.People)
		        {
			        var ppl = repo.Repository.ReadAll().FirstOrDefault(e => person.Id == e.Id);
			        if (ppl != null)
			        {
				        //Person with ID already exists in DB, add to address
				        addr.People.Add(ppl);
			        }
			        else
			        {
				        //Person does not exist, add to DB and address
				        repo.Repository.Create(person);
				        addr.People.Add(person);
			        }
		        }

		        repo.Commit();
	        }

	        addr.AddressType = publicAddresses.AddressType;
	        addr.City = publicAddresses.City;
	        addr.HouseNumber = publicAddresses.HouseNumber;
	        addr.StreetName = publicAddresses.StreetName;
	        addr.ZipCode = publicAddresses.ZipCode;

	        uow.Commit();

			
			
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
		        ZipCode = p.ZipCode,
	            People = (from ppl in p.People
	                select ppl.LastName + ", " + ppl.FirstName)
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