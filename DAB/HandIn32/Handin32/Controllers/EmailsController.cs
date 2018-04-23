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
    public class EmailsController : ApiController
    {
        private AddressModel db = new AddressModel();

        // GET: api/Emails
        public IQueryable<EmailDto> GetEmails()
        {
            var uow = new UnitOfWork<Emails>(db);

            var email = from e in uow.Repository.ReadAll()
                        select new EmailDto
                        {
                            Id = e.Id,
                            MailAddress = e.MailAddress,
                            PersonId = e.Person_Id
                        };

            return email;
        }

        // GET: api/Emails/5
        [ResponseType(typeof(Emails))]
        public async Task<IHttpActionResult> GetEmails(int id)
        {
            var uow = new UnitOfWork<Emails>(db);
            Emails emails = uow.Repository.Read(id);
            if (emails == null)
            {
                return NotFound();
            }

            return Ok(emails);
        }

        // PUT: api/Emails/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutEmails(int id, Emails emails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != emails.Id)
            {
                return BadRequest();
            }

            db.Entry(emails).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmailsExists(id))
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

        // POST: api/Emails
        [ResponseType(typeof(Emails))]
        public async Task<IHttpActionResult> PostEmails(Emails emails)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Emails.Add(emails);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = emails.Id }, emails);
        }

        // DELETE: api/Emails/5
        [ResponseType(typeof(Emails))]
        public async Task<IHttpActionResult> DeleteEmails(int id)
        {
            Emails emails = await db.Emails.FindAsync(id);
            if (emails == null)
            {
                return NotFound();
            }

            db.Emails.Remove(emails);
            await db.SaveChangesAsync();

            return Ok(emails);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool EmailsExists(int id)
        {
            return db.Emails.Count(e => e.Id == id) > 0;
        }
    }
}