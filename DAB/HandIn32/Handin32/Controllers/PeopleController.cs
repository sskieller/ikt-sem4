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
    public class PeopleController : ApiController
    {
        private AddressModel db = new AddressModel();

        // GET: api/People
        public IQueryable<PeopleDto> GetPeople()
        {
	        var uow = new UnitOfWork<People>(db);

	        var people = from p in uow.Repository.ReadAll()
		        select new PeopleDto
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
		        };

	        return people;


        }

        // GET: api/People/5
        [ResponseType(typeof(People))]
        public async Task<IHttpActionResult> GetPeople(int id)
        {
	        var uow = new UnitOfWork<People>(db);
		    People people = uow.Repository.Read(id);

		    if (people == null)
			    return NotFound();

		    return Ok(people);
	        
        }

        // PUT: api/People/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPeople(int id, People people)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != people.Id)
            {
                return BadRequest();
            }

            db.Entry(people).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PeopleExists(id))
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

        // POST: api/People
        [ResponseType(typeof(People))]
        public async Task<IHttpActionResult> PostPeople(People people)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.People.Add(people);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = people.Id }, people);
        }

        // DELETE: api/People/5
        [ResponseType(typeof(People))]
        public async Task<IHttpActionResult> DeletePeople(int id)
        {
            People people = await db.People.FindAsync(id);
            if (people == null)
            {
                return NotFound();
            }

            db.People.Remove(people);
            await db.SaveChangesAsync();

            return Ok(people);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PeopleExists(int id)
        {
            return db.People.Count(e => e.Id == id) > 0;
        }
    }
}