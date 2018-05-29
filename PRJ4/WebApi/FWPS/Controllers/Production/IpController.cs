using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// Controller responsible for getting and updating Master Unit IP Address
    /////////////////////////////////////////////////
    [Route("api/[Controller]")]
	public class IpController : Controller
	{
		private readonly FwpsDbContext _context;

		public IpController(FwpsDbContext context)
		{
			_context = context;

			if (_context.IpItems.Any() == false)
			{
				_context.IpItems.Add(new IpItem() { Ip = "0.0.0.0" });
				_context.SaveChanges();
			}
		}

	    /////////////////////////////////////////////////
	    /// Not used, Gets all IP items from database
	    /////////////////////////////////////////////////
        [HttpGet]
		public IEnumerable<IpItem> GetAll()
		{
			return _context.IpItems.ToList();

		}

	    /////////////////////////////////////////////////
	    /// Gets IP item denoted by {id}
	    /////////////////////////////////////////////////
        [HttpGet("{id:int}", Name = "Getip")]
		public IActionResult GetById(long id)
		{
			var item = _context.IpItems.FirstOrDefault(t => t.Id == id);
			{
				if (item == null)
				{
					return NotFound();
				}
			}
			return new ObjectResult(item);
		}


	    /////////////////////////////////////////////////
	    /// Not used, Creates new IP item in database
	    /////////////////////////////////////////////////
        [HttpPost]
		public IActionResult Create([FromBody] IpItem item)
		{
			if (item == null)
				return BadRequest();


			_context.IpItems.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("Getip", new { id = item.Id }, item);
		}

	    /////////////////////////////////////////////////
	    /// Replaces IP item denoted by {id} with IP item passed in parameter
	    /////////////////////////////////////////////////
        [HttpPut("{id}")]
		public IActionResult Update(long id, [FromBody] IpItem item)
		{
			if (item == null || item.Id != id)
			{
				return BadRequest();
			}

			var ip = _context.IpItems.FirstOrDefault(o => o.Id == id);
			if (ip == null)
			{
				return NotFound();
			}

			ip.Ip = item.Ip;
			ip.LastModifiedDate = DateTime.Now;

			_context.IpItems.Update(ip);
			_context.SaveChanges();
			return new NoContentResult();
		}

	    /////////////////////////////////////////////////
	    /// Not used, Deletes IP item denoted by {id}
	    /////////////////////////////////////////////////
        [HttpDelete("{id}")]
		public IActionResult Delete(long id)
		{
			var ip = _context.IpItems.FirstOrDefault(o => o.Id == id);
			if (ip == null)
			{
				return NotFound();
			}

			_context.IpItems.Remove(ip);
			_context.SaveChanges();
			return new NoContentResult();
		}
	}
}
