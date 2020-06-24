using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Writers;
using Newtonsoft.Json;
using playing.Core.Routes;
using Swashbuckle.AspNetCore.SwaggerGen;
using Endpoint = playing.Controllers.Models.Endpoint;

namespace playing.Controllers
{
    [Route("_routes")]
    [ApiController]
    public class RoutesController : ControllerBase
    {
        // for accessing conventional routes...
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IRoutesLogic _routesLogic;

        public RoutesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _routesLogic = new RoutesLogic(this);
        }

        [HttpGet]
        public IActionResult Index()
        {
            var list = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(x=>!x.AttributeRouteInfo.Template.StartsWith('_'))
                .Select(ad => new Endpoint
                {
                    method = _routesLogic.GetMethodType(ad), 
                    path = _routesLogic.GetActionPath(ad), 
                    name = _routesLogic.GetActionName(ad),
                    description = _routesLogic.GetDescription(ad)
                }).ToList();

            return Ok(list);
        }
    }
}
