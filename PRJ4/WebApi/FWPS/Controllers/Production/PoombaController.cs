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
    /////////////////////////////////////////////////
    /// Controller responsible for Poomba API
    /////////////////////////////////////////////////
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

        /////////////////////////////////////////////////
        /// Returns all Poomba items from database
        /////////////////////////////////////////////////
        [HttpGet]
        public IEnumerable<PoombaItem> GetAll()
        {
            return _context.PoombaItems.ToList();

        }


        /////////////////////////////////////////////////
        /// Returns Poomba item denoted by {id}
        /////////////////////////////////////////////////
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

        /////////////////////////////////////////////////
        /// Returns all items within 7 days of current date
        /////////////////////////////////////////////////
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

        /////////////////////////////////////////////////
        /// Returns newest Poomba item from database
        /////////////////////////////////////////////////
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

        /////////////////////////////////////////////////
        /// Creates new Poomba item in database
        /////////////////////////////////////////////////
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
    }
}
