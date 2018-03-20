using System;
using System.Collections.Generic;
using System.Linq;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using FWPS.Data;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FWPS.Controllers
{
    [Route("api/[Controller]")]
    public class LightController : Controller
    {
        private readonly FwpsDbContext _context;

        public LightController(FwpsDbContext context)
        {
            _context = context;

            if (_context.LightItems.Any() == false)
            {
                _context.LightItems.Add(new LightItem() { Command = "on", IsRun = false });
                _context.SaveChanges();
            }
        }

        [HttpGet("[action]")]
        public ActionResult Index()
        {
            var lightItems = _context.LightItems.ToList();
            return View(lightItems);
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


        //[HttpPost]
        //public IActionResult Create([FromBody] LightItem lightItem)
        //{
        //    if (lightItem == null)
        //        return BadRequest();


        //    _context.LightItems.Add(lightItem);
        //    _context.SaveChanges();

        //    return CreatedAtRoute("GetLight", new { id = lightItem.Id }, lightItem);
        //}

        [HttpPost]
        public IActionResult Create([FromBody] LightItem lightItem)
        {
            if (lightItem == null)
            {
                return BadRequest();
            }

            

            var light = new LightItem
            {
                Command = lightItem.Command,
                IsRun = lightItem.IsRun,
                CreatedDate = DateTime.Now,
                LastModifiedDate = DateTime.Now
            };

            _context.LightItems.Add(light);
            _context.SaveChanges();


            return CreatedAtRoute("GetLight", new {id = light.Id}, light);
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
