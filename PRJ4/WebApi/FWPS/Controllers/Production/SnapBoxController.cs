using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{


    /////////////////////////////////////////////////
    /// Controller responsible for SnapBox API
    /////////////////////////////////////////////////
    [Route("api/[Controller]")]
	public class SnapBoxController : Controller, ISnapBoxController
	{
		private readonly FwpsDbContext _context;
	    private List<IObserver<SnapBoxItem>> _observers;

		public SnapBoxController(FwpsDbContext context)
		{
		    _context = context ?? throw new NullReferenceException();
		    _observers = new List<IObserver<SnapBoxItem>>();

			if (_context.SnapBoxItems.Any() == false)
			{
				_context.SnapBoxItems.Add(new SnapBoxItem { PowerLevel = "100", MailReceived = false, ReceiverEmail = "simonvu@post.au.dk", SnapBoxId = "000"});
				_context.SaveChanges();
			}
		}

	    /////////////////////////////////////////////////
	    /// Returns all SnapBox items in database
	    /////////////////////////////////////////////////
        [HttpGet]
		public IEnumerable<SnapBoxItem> GetAll()
		{
            if (_context.SnapBoxItems.ToList().Count <= 0)
            {
                throw new NoItemsInDatabaseException();
            }
			return _context.SnapBoxItems.ToList();
		}
	    /////////////////////////////////////////////////
	    /// Returns all items from database no older than 7 days from current date
	    /////////////////////////////////////////////////
        [HttpGet("[action]")] // '/api/snapbox/getupdate'
	    public IEnumerable<SnapBoxItem> GetUpdate()
	    {
	        if (_context.SnapBoxItems.ToList().Count <= 0)
	        {
	            throw new NoItemsInDatabaseException();
	        }

	        var items = from it in _context.SnapBoxItems
	            where DateTime.Now.Subtract(it.CreatedDate) < TimeSpan.FromDays(7)
	            select it;

	        return items;
	    }

	    /////////////////////////////////////////////////
	    /// Test method, Clears database from items older than 7 days and creates 20 test elements
	    /////////////////////////////////////////////////
        [HttpGet("[action]")] // '/api/snapbox/Testupdate'
	    public IEnumerable<SnapBoxItem> TestUpdate()
	    {
	        if (_context.SnapBoxItems.ToList().Count <= 0)
	        {
	            throw new NoItemsInDatabaseException();
	        }

	        var items = from it in _context.SnapBoxItems
	            where DateTime.Now.Subtract(it.CreatedDate) < TimeSpan.FromDays(7)
                select it;

	        foreach (var item in items)
	        {
	            _context.SnapBoxItems.Remove(item);
	        }

	        DateTime[] dates = new DateTime[20];
            List<int> daysFromNow = new List<int>(20)
            {
                6, 6, 5, 5, 5, 4, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3, 3,2, 1, 1
            };
            for (int i = 0; i < 20; ++i)
            {
                dates[i] = DateTime.Now - TimeSpan.FromDays(daysFromNow[i]);
                _context.SnapBoxItems.Add(new SnapBoxItem()
	            {
                    Checksum = "TEST",
                    CreatedDate = dates[i],
                    LastModifiedDate = dates[i],
                    MailReceived = false,
                    PowerLevel = "100",
                    ReceiverEmail = "",
                    SnapBoxId = "0xTEST"
                });
	        }

            _context.SaveChanges();

            return items;
	    }


	    /////////////////////////////////////////////////
	    /// Gets SnapBox item denoted by {id}
	    /////////////////////////////////////////////////
        [HttpGet("{id:int}", Name = "GetSnapBox")]
		public IActionResult GetById(long id)
		{
			var item = _context.SnapBoxItems.FirstOrDefault(t => t.Id == id);
			{
				if (item == null)
				{
					return NotFound();
				}
			}
			return new ObjectResult(item);
		}

	    /////////////////////////////////////////////////
	    /// Gets newest SnapBox item from database
	    /////////////////////////////////////////////////
        [HttpGet("[action]")] // '/api/snapbox/Newest'
	    public IActionResult Newest()
	    {
	        var item = _context.SnapBoxItems.Last();
	        if (item == null)
	        {
	            return NotFound();
	        }
	        return new ObjectResult(item);
	    }

	    /////////////////////////////////////////////////
	    /// Creates new SnapBox item in database
	    /////////////////////////////////////////////////
        [HttpPost]
		public IActionResult Create([FromBody] SnapBoxItem item)
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
                //MailSender ms = new MailSender(_context);
                //ms.SendSnapBoxMail(item);
            }

			return CreatedAtRoute("GetSnapBox", new { id = item.Id }, item);
		}

	    /////////////////////////////////////////////////
	    /// Not used, Deletes SnapBox item denoted by {id}
	    /////////////////////////////////////////////////
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var item = _context.SnapBoxItems.FirstOrDefault(o => o.Id == id);
			if (item == null)
			{
				return NotFound();
			}

			_context.SnapBoxItems.Remove(item);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
