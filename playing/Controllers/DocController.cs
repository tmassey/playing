using System;
using System.IO;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Swagger;

namespace playing.Controllers
{
    [Route("_doc")]
    [ApiController]
    public class DocController : ControllerBase
    {
        private ISwaggerProvider _provider;

        public DocController(ISwaggerProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        public IActionResult Index()
        {
            
            var sb = new StringBuilder();
            var txtWriter = new StringWriter(sb);
            IOpenApiWriter writer = new OpenApiJsonWriter(txtWriter);
            _provider.GetSwagger("v1", "http://localhost:17209").SerializeAsV3(writer);
            return Ok(sb.ToString());
        }
    }
}