using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using playing.Authorization.Interfaces;
using playing.Core.Attributes;
using playing.Core.Extentions;

namespace playing.Controllers
{
    [Route("api/[controller]")]
    [Authorize(policy: "AtLeast21")]
    public class ValuesController : PhoenixBaseController
    {
        // GET api/values
        [HttpGet("/api/Values/")]
        [Description("Get Values")]
        public IEnumerable<string> Get()
        {
            
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("/api/Values/{id}")]
        [Description("Get individual Value")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost("/api/Values/")]
        [Description("Post Values")]
        public void Post([FromBody]SomeModel value)
        {
        }
        
        [HttpPost]
        [Route("/api/Values/PerformAnAction")]
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
    public class SomeModel{
        public int id { get; set; }
        public string desc { get; set; }
    }
}
