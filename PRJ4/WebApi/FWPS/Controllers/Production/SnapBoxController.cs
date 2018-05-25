using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    public class NoItemsInDatabaseException : Exception
    {
    }
    
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

	    [HttpGet]
		public IEnumerable<SnapBoxItem> GetAll()
		{
            if (_context.SnapBoxItems.ToList().Count <= 0)
            {
                throw new NoItemsInDatabaseException();
            }
			return _context.SnapBoxItems.ToList();
		}

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
                MailSender ms = new MailSender(_context);
                ms.SendSnapBoxMail(item);
            }

			return CreatedAtRoute("GetSnapBox", new { id = item.Id }, item);
		}

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
