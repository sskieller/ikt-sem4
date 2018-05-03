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
    public class HodoorController : Controller
    {
        private readonly FwpsDbContext _context;
        // SignalRHubContext
        private IHubContext<DevicesHub> _hub;


        public HodoorController(FwpsDbContext context, IHubContext<DevicesHub> hub)
        {
            _hub = hub;
            _context = context;

            if (_context.HodoorItems.Any() == false)
            {
                _context.HodoorItems.Add(new HodoorItem() { Command = "CmdUnlock", OpenStatus = false});
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<HodoorItem> GetAll()
        {
            return _context.HodoorItems.ToList();

        }

        [HttpGet("{id:int}", Name = "GetHodoor")]
        public IActionResult GetById(long id)
        {
            var hodoorItem = _context.HodoorItems.FirstOrDefault(t => t.Id == id);
            {
                if (hodoorItem == null)
                {
                    return NotFound();
                }
            }
            return new ObjectResult(hodoorItem);
        }

        [HttpGet("[action]")] // '/api/Light/next'
        public IActionResult Next()
        {
            var hodoorItem = _context.HodoorItems.LastOrDefault(o => o.IsRun == false && o.Command != null);
            if (hodoorItem == null)
            {
                return NotFound();
            }
            return new ObjectResult(hodoorItem);
        }


	    [HttpGet("[action]")] // '/api/Light/Newest'
	    public IActionResult Newest()
	    {
		    var hodoorItem = _context.HodoorItems.Last();
		    if (hodoorItem == null)
		    {
			    return NotFound();
		    }
		    return new ObjectResult(hodoorItem);
	    }

		[HttpPost]
        public IActionResult Create([FromBody] HodoorItem hodoorItem)
        {
            if (hodoorItem == null)
            {
                return BadRequest();
            }
  

            _context.HodoorItems.Add(hodoorItem);
            _context.SaveChanges();

			DebugWriter.Write(string.Format("Created HodoorItem with ID: {0}", hodoorItem.Id));

            try
            {
                _hub.Clients.All.InvokeAsync("UpdateSpecific", "Hodoor", hodoorItem.Command, hodoorItem);

            }
            catch (Exception e)
            {
                DebugWriter.Write(e.Message);
            }

            return CreatedAtRoute("GetLight", new {id = hodoorItem.Id}, hodoorItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] HodoorItem hodoorItem)
        {
            if (hodoorItem == null || hodoorItem.Id != id)
            {
                return BadRequest();
            }

            var hodoor = _context.HodoorItems.FirstOrDefault(o => o.Id == id);
            if (hodoor == null)
            {
                return NotFound();
            }

            hodoor.IsRun = hodoorItem.IsRun;
            hodoor.LastModifiedDate = DateTime.Now;
            hodoor.Command = hodoorItem.Command;
            hodoor.OpenStatus = hodoorItem.OpenStatus;

            _context.HodoorItems.Update(hodoor);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var hodoorItem = _context.HodoorItems.FirstOrDefault(o => o.Id == id);
            if (hodoorItem == null)
            {
                return NotFound();
            }

            _context.HodoorItems.Remove(hodoorItem);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
