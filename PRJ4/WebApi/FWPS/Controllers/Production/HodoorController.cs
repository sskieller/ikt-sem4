using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using FWPS.Data;
using Microsoft.AspNetCore.SignalR;
using NSubstitute;
using NSubstitute.Extensions;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FWPS.Controllers
{
    interface IHodoorController
    {
        [HttpGet]
        IEnumerable<HodoorItem> GetAll();

        [HttpGet("{id:int}", Name = "GetHodoor")]
        IActionResult GetById(long id);

        [HttpGet("[action]")] // '/api/Light/next'
        IActionResult Next();

        [HttpGet("[action]")] // '/api/Light/Newest'
        IActionResult Newest();

        [HttpPost]
        IActionResult Create([FromBody] HodoorItem hodoorItem);

        [HttpPut("{id}")]
        IActionResult Update(long id, [FromBody] HodoorItem hodoorItem);

        [HttpDelete("{id}")]
        IActionResult Delete(long id);
    }

    [Route("api/[Controller]")]
    public class MockHodoorController : Controller, IHodoorController
    {
        private readonly FwpsDbContext _context;
        // SignalRHubContext
        private IHubContext<DevicesHub> _hub;

        private IDebugWriter _stubDebugWriter;

        public MockHodoorController(FwpsDbContext context, IHubContext<DevicesHub> hub)
        {
            _hub = hub ?? throw new NullReferenceException();
            _context = context ?? throw new NullReferenceException();

            if (_context.HodoorItems.Any() == false)
            {
                _context.HodoorItems.Add(new HodoorItem() { Command = "CmdUnlock", OpenStatus = false });
                _context.SaveChanges();
            }
        }

        public IEnumerable<HodoorItem> GetAll()
        {
            throw new NotImplementedException();
        }

        public IActionResult GetById(long id)
        {
            throw new NotImplementedException();
        }

        public IActionResult Next()
        {
            throw new NotImplementedException();
        }

        public IActionResult Newest()
        {
            throw new NotImplementedException();
        }

        public IActionResult Create(HodoorItem hodoorItem)
        {
            if (hodoorItem == null)
            {
                return BadRequest();
            }

            _stubDebugWriter = new StubDebugWriter();

            _context.HodoorItems.Add(hodoorItem);
            _context.SaveChanges();

            _stubDebugWriter.Write(string.Format("Created HodoorItem with ID: {0}", hodoorItem.Id));

            try
            {
                _hub.Clients.All.InvokeAsync("UpdateSpecific", "Hodoor", hodoorItem.Command, hodoorItem);

            }
            catch (Exception e)
            {
                _stubDebugWriter.Write(e.Message);
            }

            return CreatedAtRoute("GetHodoor", new { id = hodoorItem.Id }, hodoorItem);
        }

        public IActionResult Update(long id, HodoorItem hodoorItem)
        {
            throw new NotImplementedException();
        }

        public IActionResult Delete(long id)
        {
            throw new NotImplementedException();
        }
    }

    [Route("api/[Controller]")]
    public class HodoorController : Controller, IHodoorController
    {
        private readonly FwpsDbContext _context;
        // SignalRHubContext
        private IHubContext<DevicesHub> _hub;

        private DebugWriter _debugWriter;

        public HodoorController(FwpsDbContext context, IHubContext<DevicesHub> hub)
        {
            _hub = hub ?? throw new NullReferenceException();
            _context = context ?? throw new NullReferenceException();

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
  
            _debugWriter = new DebugWriter();

            _context.HodoorItems.Add(hodoorItem);
            _context.SaveChanges();

			_debugWriter.Write(string.Format("Created HodoorItem with ID: {0}", hodoorItem.Id));

            try
            {
                _hub.Clients.All.InvokeAsync("UpdateSpecific", "Hodoor", hodoorItem.Command, hodoorItem);
            }
            catch (Exception e)
            {
                _debugWriter.Write(e.Message);
            }

            return CreatedAtRoute("GetHodoor", new {id = hodoorItem.Id}, hodoorItem);
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
