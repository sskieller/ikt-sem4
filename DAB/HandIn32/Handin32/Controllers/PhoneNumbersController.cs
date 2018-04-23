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

namespace Handin32.Controllers
{
    public class PhoneNumbersController : ApiController
    {
        private AddressModel db = new AddressModel();

        // GET: api/PhoneNumbers
        public IQueryable<PhoneDto> GetPhoneNumbers()
        {
            var uow = new UnitOfWork<PhoneNumbers>(db);
	        var phone = from p in uow.Repository.ReadAll()
				.Include(p => p.People)
		        select new PhoneDto
		        {
			        Id = p.Id,
			        PhoneCompany = p.PhoneCompany,
					PersonId = p.Person_Id,
					PhoneType = p.PhoneType,
					PersonName = p.People.LastName + ", " + p.People.FirstName
		        };

	        return phone;

        }

        // GET: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneDto))]
        public async Task<IHttpActionResult> GetPhoneNumbers(int id)
        {
	        var uow = new UnitOfWork<PhoneNumbers>(db);
	        var phone = uow.Repository.Read(id);

	        if (phone == null)
		        return NotFound();

	        PhoneDto dto = uow.Repository.ReadAll()
		        .Include(p => p.People)
		        .Select(p => new PhoneDto()
		        {
			        Id = p.Id,
			        PhoneCompany = p.PhoneCompany,
			        PersonId = p.Person_Id,
			        PhoneType = p.PhoneType,
			        PersonName = p.People.LastName + ", " + p.People.FirstName

		        }).SingleOrDefault(p => p.Id == id);

			return Ok(dto);
		}

        // PUT: api/PhoneNumbers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoneNumbers(int id, PhoneNumbers phoneNumbers)
        {
			/*
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != phoneNumbers.Id)
            {
                return BadRequest();
            }

            db.Entry(phoneNumbers).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhoneNumbersExists(id))
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

        // POST: api/PhoneNumbers
        [ResponseType(typeof(PhoneDto))]
        public async Task<IHttpActionResult> PostPhoneNumbers(PhoneNumbers phoneNumbers)
        {
	        var uow = new UnitOfWork<PhoneNumbers>(db);
			if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

			uow.Repository.Create(phoneNumbers);
	        uow.Commit();

	        PhoneDto dto = uow.Repository.ReadAll()
		        .Include(p => p.People)
		        .Select(p => new PhoneDto()
		        {
			        Id = p.Id,
			        PhoneCompany = p.PhoneCompany,
			        PersonId = p.Person_Id,
			        PhoneType = p.PhoneType,
			        PersonName = p.People.LastName + ", " + p.People.FirstName

		        }).SingleOrDefault(p => p.Id == phoneNumbers.Id);


			return CreatedAtRoute("DefaultApi", new { id = phoneNumbers.Id }, dto);
        }

        // DELETE: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneNumbers))]
        public async Task<IHttpActionResult> DeletePhoneNumbers(int id)
        {
            var uow = new UnitOfWork<PhoneNumbers>(db);
	        var phoneNumbers = uow.Repository.Read(id);
            if (phoneNumbers == null)
            {
                return NotFound();
            }

			uow.Repository.Delete(phoneNumbers);
	        uow.Commit();
			
            return Ok(phoneNumbers);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PhoneNumbersExists(int id)
        {
            return db.PhoneNumbers.Count(e => e.Id == id) > 0;
        }
    }
}