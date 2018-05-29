using System.Collections.Generic;
using FWPS.Models;
using Microsoft.AspNetCore.Mvc;

namespace FWPS.Controllers
{
    /////////////////////////////////////////////////
    /// Interface for HodoorController 
    /////////////////////////////////////////////////
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
}