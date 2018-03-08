using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FWPS.Controllers
{
    [Route("api/[Controller]")]
    public class LightController : Controller
    {
        private readonly LightContext _context;

        public LightController(LightContext context)
        {
            _context = context;

            if (_context.LightItems.Any() == false)
            {
                _context.LightItems.Add(new LightItem() {Command = "on", IsRun = false});
                _context.SaveChanges();
            }
        }

	    [HttpGet("[action]")]
		public ActionResult Index()
	    {
		    var items = _context.LightItems.ToList();
		    return View(items);
	    }

        [HttpGet]
        public IEnumerable<LightItem> GetAll()
        {
            return _context.LightItems.ToList();
			
        }


		[HttpGet("{id:int}", Name = "GetLight")]
        public IActionResult GetById(long id)
        {
            var item = _context.LightItems.FirstOrDefault(t => t.Id == id);
            {
                if (item == null)
                {
                    return NotFound();
                }
            }
            return new ObjectResult(item);
        }

	    [HttpGet("[action]")] // '/api/Light/next'
	    public IActionResult Next()
	    {
		    var item = _context.LightItems.LastOrDefault(o => o.IsRun == false);
		    if (item == null)
		    {
			    return NotFound();
		    }
		    return new ObjectResult(item);
	    }


		[HttpPost]
        public IActionResult Create([FromBody] LightItem item)
        {
            if (item == null)
                return BadRequest();


            _context.LightItems.Add(item);
            _context.SaveChanges();

            return CreatedAtRoute("GetLight", new {id = item.Id}, item);
        }
        [HttpPut("{id}")]
        public IActionResult Update(long id, [FromBody] LightItem item)
        {
            if (item == null || item.Id != id)
            {
                return BadRequest();
            }

            var light = _context.LightItems.FirstOrDefault(o => o.Id == id);
            if (light == null)
            {
                return NotFound();
            }

            light.IsRun = item.IsRun;
            light.LastModifiedDate = DateTime.Now;

            _context.LightItems.Update(light);
            _context.SaveChanges();
            return new NoContentResult();
        }
		

        [HttpDelete("{id}")]
        public IActionResult Delete(long id)
        {
            var light = _context.LightItems.FirstOrDefault(o => o.Id == id);
            if (light == null)
            {
                return NotFound();
            }

            _context.LightItems.Remove(light);
            _context.SaveChanges();
            return new NoContentResult();
        }
    }
}
