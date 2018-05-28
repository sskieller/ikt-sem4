using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using FWPS.Data;
using Microsoft.AspNetCore.SignalR;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FWPS.Controllers
{
    [Route("api/[Controller]")]
    public class PoombaController : Controller
    {
        private readonly FwpsDbContext _context;
        // SignalRHubContext
        private IHubContext<DevicesHub> _hub;


        public PoombaController(FwpsDbContext context, IHubContext<DevicesHub> hub)
        {
            _hub = hub;
            _context = context;

            if (_context.PoombaItems.Any() == false)
            {
                _context.PoombaItems.Add(new PoombaItem() { Command = "on", IsRun = false });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<PoombaItem> GetAll()
        {
            return _context.PoombaItems.ToList();

        }

        [HttpGet("{id:int}", Name = "GetPoomba")]
        public IActionResult GetById(long id)
        {
            var poombaItem = _context.PoombaItems.FirstOrDefault(t => t.Id == id);
            {
                if (poombaItem == null)
                {
                    return NotFound();
                }
            }
            return new ObjectResult(poombaItem);
        }

        [HttpGet("[action]")] // '/api/Poomba/getupdate'
        public IEnumerable<PoombaItem> GetUpdate()
        {
            if (_context.PoombaItems.ToList().Count <= 0)
            {
                throw new NoItemsInDatabaseException();
            }

            var items = from it in _context.PoombaItems
                where DateTime.Now.Subtract(it.CreatedDate) < TimeSpan.FromDays(7)
                select it;

            return items;
        }

        [HttpGet("[action]")] // '/api/Poomba/next'
        public IActionResult Next()
        {
            var poombaItem = _context.PoombaItems.LastOrDefault(o => o.IsRun == false && o.Command != null);
            if (poombaItem == null)
            {
                return NotFound();
            }
            return new ObjectResult(poombaItem);
        }


	    [HttpGet("[action]")] // '/api/Poomba/Newest'
	    public IActionResult Newest()
	    {
		    var poombaItem = _context.PoombaItems.Last();
		    if (poombaItem == null)
		    {
			    return NotFound();
		    }
		    return new ObjectResult(poombaItem);
	    }

		[HttpPost]
        public IActionResult Create([FromBody] PoombaItem poombaItem)
        {
            if (poombaItem == null)
            {
                return BadRequest();
            }

	        if (poombaItem.WakeUpTime == DateTime.MinValue)
		        poombaItem.WakeUpTime = _context.PoombaItems.Last().WakeUpTime;

	        if (poombaItem.SleepTime == DateTime.MinValue)
		        poombaItem.SleepTime = _context.PoombaItems.Last().SleepTime;          

            _context.PoombaItems.Add(poombaItem);
            _context.SaveChanges();

            new DebugWriter().Write(string.Format("Created Poombaitem with ID: {0}", poombaItem.Id));

            try
            {
                _hub.Clients.All.InvokeAsync("UpdateSpecific", "Poomba", poombaItem.Command, poombaItem);

            }
            catch (Exception e)
            {
                new DebugWriter().Write(e.Message);
            }

            return CreatedAtRoute("GetPoomba", new {id = poombaItem.Id}, poombaItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] PoombaItem poombaItem)
        {
            if (poombaItem == null || poombaItem.Id != id)
            {
                return BadRequest();
            }

            var poomba = _context.PoombaItems.FirstOrDefault(o => o.Id == id);
            if (poomba == null)
            {
                return NotFound();
            }

            poomba.IsRun = poombaItem.IsRun;
            poomba.LastModifiedDate = DateTime.Now;
	        poomba.Command = poombaItem.Command;
	        if (poombaItem.SleepTime != DateTime.MinValue && poombaItem.WakeUpTime != DateTime.MinValue)
	        {
		        poomba.SleepTime = poombaItem.SleepTime;
		        poomba.WakeUpTime = poombaItem.WakeUpTime;
	        }

            _context.PoombaItems.Update(poomba);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var poombaItem = _context.PoombaItems.FirstOrDefault(o => o.Id == id);
            if (poombaItem == null)
            {
                return NotFound();
            }

            _context.PoombaItems.Remove(poombaItem);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
