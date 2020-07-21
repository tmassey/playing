using System.IO;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Writers;
using Swashbuckle.AspNetCore.Annotations;
using Swashbuckle.AspNetCore.Swagger;

namespace Phoenix.Api.Core.Controllers
{
    [Route("_moduledocs")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class DocController : ControllerBase
    {
        private readonly ISwaggerProvider _provider;

        public DocController(ISwaggerProvider provider)
        {
            _provider = provider;
        }

        [HttpGet]
        [SwaggerResponse(200, "swagger Docs", typeof(string))]
        public string Index()
        {
            
            var sb = new StringBuilder();
            var txtWriter = new StringWriter(sb);
            IOpenApiWriter writer = new OpenApiJsonWriter(txtWriter);
            _provider.GetSwagger("v1").SerializeAsV3(writer);
            return sb.ToString();
        }
    }
}