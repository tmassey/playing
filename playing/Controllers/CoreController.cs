using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using playing.Authorization.Interfaces;
using playing.Core.Extentions;

namespace playing.Controllers
{
    [Route("/")]
    [ApiController]
    public class CoreController : PhoenixBaseController
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

        public CoreController(IUserManager userManager) : base(userManager)
        {
        }
    }
}