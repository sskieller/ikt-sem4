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
    public class LoginController : Controller
    {
        private readonly FwpsDbContext _context;

        public LoginController(FwpsDbContext context)
        {
            _context = context;

            if (_context.LoginItems.Any() == false)
            {
                _context.LoginItems.Add(new LoginItem() { Username = "User", Password = "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a" });
                _context.SaveChanges();
            }
        }


        [HttpGet("{RFID}", Name = "GetRFID")]
        public IActionResult GetByUsername(string rfid)
        {
            var item = _context.LoginItems.FirstOrDefault(t => t.Rfid == rfid);
            {
                if (item == null)
                {
                    return NotFound();
                }
            }
            return Ok();
        }


        [HttpGet("{username};{password}", Name = "GetLogin")]
        public IActionResult GetAllowLogin(string username, string password)
        {
            var item = _context.LoginItems.FirstOrDefault(t => (t.Username == username & t.Password == password));
            {
                if (item == null)
                {
                    return NotFound();
                }
            }
            return Content("Hi there");
        }
        
        
    }
}
