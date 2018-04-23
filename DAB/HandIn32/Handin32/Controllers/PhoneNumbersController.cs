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
		        select new PhoneDto
		        {
			        Id = p.Id,
			        PhoneCompany = p.PhoneCompany,
					PersonId = p.Person_Id,
					PhoneType = p.PhoneType
		        };

	        return phone;

        }

        // GET: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneDto))]
        public async Task<IHttpActionResult> GetPhoneNumbers(int id)
        {
	        var uow = new UnitOfWork<People>(db);
	        PeopleDto people = uow.Repository.ReadAll()
		        .Include(p => p.Emails).Include(p => p.PhoneNumbers).Include(p => p.PublicAddresses)
		        .Select(p => new PeopleDto()
		        {
			        Id = p.Id,
			        Emails = from e in p.Emails
				        select new EmailDto
				        {
					        Id = e.Id,
					        MailAddress = e.MailAddress
				        },
			        Firstname = p.FirstName,
			        LastName = p.LastName,
			        PhoneNumbers = from n in p.PhoneNumbers
				        select new PhoneDto
				        {
					        Id = n.Id,
					        PhoneCompany = n.PhoneCompany,
					        PhoneType = n.PhoneType
				        },
			        PublicAddresses = from a in p.PublicAddresses
				        select new PublicAddressDto
				        {
					        Id = a.Id,
					        HouseNumber = a.HouseNumber,
					        StreetName = a.StreetName
				        }
		        }).SingleOrDefault(p => p.Id == id);

	        if (people == null)
		        return NotFound();

	        return Ok(people);
		}

        // PUT: api/PhoneNumbers/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPhoneNumbers(int id, PhoneNumbers phoneNumbers)
        {
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

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PhoneNumbers
        [ResponseType(typeof(PhoneNumbers))]
        public async Task<IHttpActionResult> PostPhoneNumbers(PhoneNumbers phoneNumbers)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PhoneNumbers.Add(phoneNumbers);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = phoneNumbers.Id }, phoneNumbers);
        }

        // DELETE: api/PhoneNumbers/5
        [ResponseType(typeof(PhoneNumbers))]
        public async Task<IHttpActionResult> DeletePhoneNumbers(int id)
        {
            PhoneNumbers phoneNumbers = await db.PhoneNumbers.FindAsync(id);
            if (phoneNumbers == null)
            {
                return NotFound();
            }

            db.PhoneNumbers.Remove(phoneNumbers);
            await db.SaveChangesAsync();

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