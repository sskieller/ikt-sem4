using System;
using System.Collections.Generic;
using System.Linq;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    public class NoItemsInDatabaseException : Exception
    {
    }
    /////////////////////////////////////////////////
    /// SnapBoxController mock for unit testing
    /////////////////////////////////////////////////
    [Route("api/[Controller]")]
    public class MockSnapBoxController : Controller
    {
        private readonly FwpsDbContext _context;
        private List<IObserver<SnapBoxItem>> _observers;

        public MockSnapBoxController(FwpsDbContext context)
        {
            _context = context ?? throw new NullReferenceException();
            _observers = new List<IObserver<SnapBoxItem>>();

            if (_context.SnapBoxItems.Any() == false)
            {
                _context.SnapBoxItems.Add(new SnapBoxItem { PowerLevel = "100", MailReceived = false, ReceiverEmail = "simonvu@post.au.dk", SnapBoxId = "000" });
                _context.SaveChanges();
            }
        }


        public IEnumerable<SnapBoxItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public IActionResult GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Create(SnapBoxItem item)
        {
            if (item == null)
                return BadRequest();

            if (item.PowerLevel == null)
                return BadRequest("Powerlevel null");
            if (item.MailReceived && item.ReceiverEmail == null)
                return BadRequest("No ReceiverEmail specified");

            _context.SnapBoxItems.Add(item);
            _context.SaveChanges();

            if (item.MailReceived)
            {
                StubMailSender sms = new StubMailSender(_context);
                sms.SendSnapBoxMail(item);
            }

            return CreatedAtRoute("GetSnapBox", new { id = item.Id }, item);
        }

        public IActionResult Delete(long id)
        {
            throw new NotImplementedException();
        }
    }
}