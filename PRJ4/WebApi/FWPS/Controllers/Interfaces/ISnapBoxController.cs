using System.Collections.Generic;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// Interface for SnapBoxController
    /////////////////////////////////////////////////
    internal interface ISnapBoxController
    {
        [Route("api/[Controller]")]
        [HttpGet]
        IEnumerable<SnapBoxItem> GetAll();

        [HttpGet("{id:int}", Name = "GetSnapBox")]
        IActionResult GetById(long id);

        [HttpPost]
        IActionResult Create([FromBody] SnapBoxItem item);

        [HttpDelete("{id}")]
        IActionResult Delete(long id);
    }
}