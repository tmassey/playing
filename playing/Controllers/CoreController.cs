using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace playing.Controllers
{
    [Route("/")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        [HttpGet]
        [Route("_ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
        [HttpGet]
        [Route("_version")]
        public IActionResult Version()
        {
            return Ok(Service.AssemblyVersion);
        }
        [HttpGet]
        [Route("_coffee")]
        public IActionResult Coffee()
        {
            return StatusCode(418, "I'm a little teapot short and stout!");
        }
    }
}