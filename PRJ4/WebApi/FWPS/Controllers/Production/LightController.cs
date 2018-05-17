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
    public class LightController : Controller
    {
        private readonly FwpsDbContext _context;
        // SignalRHubContext
        private IHubContext<DevicesHub> _hub;


        public LightController(FwpsDbContext context, IHubContext<DevicesHub> hub)
        {
            _hub = hub;
            _context = context;

            if (_context.LightItems.Any() == false)
            {
                _context.LightItems.Add(new LightItem() { Command = "on", IsRun = false });
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<LightItem> GetAll()
        {
            return _context.LightItems.ToList();

        }

        [HttpGet("{id:int}", Name = "GetLight")]
        public IActionResult GetById(long id)
        {
            var lightItem = _context.LightItems.FirstOrDefault(t => t.Id == id);
            {
                if (lightItem == null)
                {
                    return NotFound();
                }
            }
            return new ObjectResult(lightItem);
        }

        [HttpGet("[action]")]
        public IEnumerable<LightItem> GetUpdate()
        {
            if (_context.LightItems.ToList().Count <= 0)
            {
                throw new NoItemsInDatabaseException();
            }

            var items = from it in _context.LightItems
                where DateTime.Now.Subtract(it.CreatedDate) < TimeSpan.FromDays(7)
                select it;

            return items;
        }

        [HttpGet("[action]")] // '/api/Light/next'
        public IActionResult Next()
        {
            var lightItem = _context.LightItems.LastOrDefault(o => o.IsRun == false && o.Command != null);
            if (lightItem == null)
            {
                return NotFound();
            }
            return new ObjectResult(lightItem);
        }


	    [HttpGet("[action]")] // '/api/Light/Newest'
	    public IActionResult Newest()
	    {
		    var lightItem = _context.LightItems.Last();
		    if (lightItem == null)
		    {
			    return NotFound();
		    }
		    return new ObjectResult(lightItem);
	    }

		[HttpPost]
        public IActionResult Create([FromBody] LightItem lightItem)
        {
            if (lightItem == null)
            {
                return BadRequest();
            }

	        if (lightItem.WakeUpTime == DateTime.MinValue)
		        lightItem.WakeUpTime = _context.LightItems.Last().WakeUpTime;

	        if (lightItem.SleepTime == DateTime.MinValue)
		        lightItem.SleepTime = _context.LightItems.Last().SleepTime;          

            _context.LightItems.Add(lightItem);
            _context.SaveChanges();

            new DebugWriter().Write(string.Format("Created Lightitem with ID: {0}", lightItem.Id));

            try
            {
                _hub.Clients.All.InvokeAsync("UpdateSpecific", "MorningSun", lightItem.Command, lightItem);

            }
            catch (Exception e)
            {
                new DebugWriter().Write(e.Message);
            }

            return CreatedAtRoute("GetLight", new {id = lightItem.Id}, lightItem);
        }

        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] LightItem lightItem)
        {
            if (lightItem == null || lightItem.Id != id)
            {
                return BadRequest();
            }

            var light = _context.LightItems.FirstOrDefault(o => o.Id == id);
            if (light == null)
            {
                return NotFound();
            }

            light.IsRun = lightItem.IsRun;
            light.LastModifiedDate = DateTime.Now;
	        light.Command = lightItem.Command;
	        if (lightItem.SleepTime != DateTime.MinValue && lightItem.WakeUpTime != DateTime.MinValue)
	        {
		        light.SleepTime = lightItem.SleepTime;
		        light.WakeUpTime = lightItem.WakeUpTime;
	        }

            _context.LightItems.Update(light);
            _context.SaveChanges();
            return new NoContentResult();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var lightItem = _context.LightItems.FirstOrDefault(o => o.Id == id);
            if (lightItem == null)
            {
                return NotFound();
            }

            _context.LightItems.Remove(lightItem);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
