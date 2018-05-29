using System;
using System.Collections.Generic;
using System.Linq;
using FWPS.Data;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// HodoorController mock for unit testing
    /////////////////////////////////////////////////
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
}