﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// Allows access to internal Debug Log on Web API
    /////////////////////////////////////////////////
    [Route("api/[Controller]")]
    public class DebugController : Controller
    {
        private IHubContext<DevicesHub> _hub;

        public DebugController(IHubContext<DevicesHub> dvhub)
        {
            _hub = dvhub;
            //FWPS.Devices.Hub = dvhub;
        }
        /////////////////////////////////////////////////
        /// Returns all data stored in Debug Log
        /////////////////////////////////////////////////
        [HttpGet]
        public IActionResult Get()
        {
            return Content(new DebugWriter().Read);
        }

        /////////////////////////////////////////////////
        /// Clears Debug Log
        /////////////////////////////////////////////////
        [HttpGet("[action]")] // '/api/Light/Newest'
        public IActionResult Clear()
        {
            new DebugWriter().Clear();
            return Ok();
        }

        /////////////////////////////////////////////////
        /// Get each device stored in Debug Log
        /////////////////////////////////////////////////
        [HttpGet("[action]")] // '/api/Light/Newest'
        public IActionResult Devices()
        {
            StringBuilder sb = new StringBuilder();

            foreach (var item in FWPS.Devices.ConnectedDevices)
            {
                sb.Append(item.Key);
                sb.Append("  --  ");
                sb.Append(item.Value);
                sb.Append("\r\n");
            }


            return Content(sb.ToString());
        }
    }
}