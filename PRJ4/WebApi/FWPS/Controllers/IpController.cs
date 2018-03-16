using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
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


		[HttpGet]
		public IEnumerable<IpItem> GetAll()
		{
			return _context.IpItems.ToList();

		}


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



		[HttpPost]
		public IActionResult Create([FromBody] IpItem item)
		{
			if (item == null)
				return BadRequest();


			_context.IpItems.Add(item);
			_context.SaveChanges();

			return CreatedAtRoute("Getip", new { id = item.Id }, item);
		}
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
