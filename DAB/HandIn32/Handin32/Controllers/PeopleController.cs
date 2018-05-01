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
									   PhoneType = n.PhoneType,
									   Number = n.Number
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
        [ResponseType(typeof(PeopleDto))]
        public async Task<IHttpActionResult> GetPeople(int id)
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
					        PhoneType = n.PhoneType,
							Number = n.Number
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

        // PUT: api/People/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPeople(int id, People people)
        {

	        if (!ModelState.IsValid)
	        {
		        return BadRequest(ModelState);
	        }

			var uow = new UnitOfWork<People>(db);
	        People person = uow.Repository.Read(id);



            if (id != people.Id || person == null)
            {
                return BadRequest();
            }

			//laves senere
	        if (people.Emails != null)
	        {
				var emails = new UnitOfWork<Emails>(db);
				person.Emails.Clear();
		        foreach (var mail in people.Emails)
		        {
			        var em = emails.Repository.ReadAll().FirstOrDefault(e => mail.Id == e.Id);
			        if (em != null)
			        {
				        //Mail with ID already exists in DB, add to person
						person.Emails.Add(em);
			        }
			        else
			        {
						//Mail does not exist, add to DB and person
				        emails.Repository.Create(mail);
						person.Emails.Add(mail);
			        }
		        }

		        emails.Commit(); //Commit changes
	        }

	        //laves senere
	        if (people.PublicAddresses != null)
	        {
		        var addr = new UnitOfWork<PublicAddresses>(db);
		        person.PublicAddresses.Clear();
		        foreach (var newAddr in people.PublicAddresses)
		        {
			        var ad = addr.Repository.ReadAll().FirstOrDefault(e => newAddr.Id == e.Id);
			        if (ad != null)
			        {
				        //Mail with ID already exists in DB, add to person
				        person.PublicAddresses.Add(ad);
						
			        }
			        else
			        {
				        //Mail does not exist, add to DB and person
				        addr.Repository.Create(newAddr);
				        person.PublicAddresses.Add(newAddr);
						
			        }
		        }

		        addr.Commit(); //Commit changes
	        }

	        if (people.PhoneNumbers != null)
	        {
				var numbers = new UnitOfWork<PhoneNumbers>(db);
				person.PhoneNumbers.Clear();

		        foreach (var number in people.PhoneNumbers)
		        {
			        var ph = numbers.Repository.ReadAll().FirstOrDefault(p => p.Id == number.Id);
			        if (ph != null)
			        {
				        person.PhoneNumbers.Add(ph);
			        }
			        else
			        {
						numbers.Repository.Create(number);
				        person.PhoneNumbers.Add(number);
			        }
		        }
			}




	        person.FirstName = people.FirstName;
	        person.MiddleName = people.MiddleName;
	        person.LastName = people.LastName;

	        uow.Commit();
			
            
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

            

	        var uow = new UnitOfWork<People>(db);
			uow.Repository.Create(people);
	        uow.Commit();

	        PeopleDto ppl = uow.Repository.ReadAll()
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
					        PhoneType = n.PhoneType,
							Number = n.Number
				        },
			        PublicAddresses = from a in p.PublicAddresses
				        select new PublicAddressDto
				        {
					        Id = a.Id,
					        HouseNumber = a.HouseNumber,
					        StreetName = a.StreetName
				        }
		        }).SingleOrDefault(p => p.Id == people.Id);

			return CreatedAtRoute("DefaultApi", new { id = people.Id }, ppl);
        }

        // DELETE: api/People/5
        [ResponseType(typeof(People))]
        public async Task<IHttpActionResult> DeletePeople(int id)
        {
			var uow = new UnitOfWork<People>(db);
	        People people = uow.Repository.Read(id);
            if (people == null)
            {
                return NotFound();
            }
			people.Emails.Clear();
			people.PhoneNumbers.Clear();
			people.PublicAddresses.Clear();
            uow.Repository.Delete(people);
	        uow.Commit();

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