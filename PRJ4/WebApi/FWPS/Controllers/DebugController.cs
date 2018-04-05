using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    [Route("api/[Controller]")]
    public class DebugController : Controller
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content(DebugWriter.Read());
        }
    }
}