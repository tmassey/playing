using Microsoft.AspNetCore.Mvc;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Models;

namespace Phoenix.Api.Core.Controllers
{
    [Route("/")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CoreController : PhoenixBaseController
    {
        [HttpGet]
        [Route("_ping")]
        public string Ping()
        {
            return "pong";
        }
        [HttpGet]
        [Route("_version")]
        public VersionInfo Version()
        {
            return Service.AssemblyVersion;
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