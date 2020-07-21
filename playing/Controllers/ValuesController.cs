using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Controllers;
using Swashbuckle.AspNetCore.Annotations;

namespace playing.Controllers
{
    [Route("/Values")]
    [Authorize(policy: "PlayingPolicy")]
    public class ValuesController : PhoenixBaseController
    {
        // GET api/values
        [HttpGet("/Values")]
        [Description("Get Values")]
        [SwaggerResponse(200, "The List of Roles",Type = typeof(IEnumerable<string>))]
        [SwaggerOperation(
            Summary = "Gets all Role Names",
            Description = "Requires scope1 privileges",
            OperationId = "Values_Get",
            Tags = new[] { "CoreValues", "Products" }
        )]
        public IEnumerable<string> Get()
        {
            
            //var zero = 1 - 1;
            //var x = 1 / zero;
            return this.CurrentPhoenixUser().Roles;
        }

        // GET api/values/5
        [HttpGet("/Values/{id}")]
        [Description("Get individual Value")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/Values/")]
        [Description("Post Values")]
        public void Post([FromBody]SomeModel value)
        {
        }
        
        [HttpPost]
        [Route("/Values/PerformAnAction")]
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

        public ValuesController(IUserManager userManager) : base(userManager)
        {
        }
    }
}
