using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Controllers;

namespace playing.Controllers
{
    [Route("/Test")]
    [Authorize(policy: "PlayingPolicy")]
    public class TestController : PhoenixBaseController
    {
        // GET api/values
        [HttpGet("/Test")]
        [Description("Get Values")]
        public IEnumerable<string> Get()
        {

            //var zero = 1 - 1;
            //var x = 1 / zero;
            return this.CurrentPhoenixUser().Roles;
        }

        // GET api/values/5
        [HttpGet("/Test/{id}")]
        [Description("Get individual Value")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/Test/")]
        [Description("Post Values")]
        public void Post([FromBody]SomeModel value)
        {
        }

        [HttpPost]
        [Route("/Test/PerformAnAction")]
        [Description("Perform an Action on Values")]
        public void PerformAnAction([FromBody]SomeModel value)
        {
        }
        // PUT api/values/5
        [HttpPut]
        [Description("update Values")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete()]
        [Description("Delete Values")]
        public void Delete(int id)
        {
        }

        public TestController(IUserManager userManager) : base(userManager)
        {
        }
    }
}