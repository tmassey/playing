using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Phoenix.Api.Core.Controllers.Models;
using Phoenix.Api.Core.Routes;

namespace Phoenix.Api.Core.Controllers
{
    [Route("_routes")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class RoutesController : PhoenixBaseController
    {
        private readonly IActionDescriptorCollectionProvider _actionDescriptorCollectionProvider;
        private readonly IRoutesLogic _routesLogic;

        public RoutesController(IActionDescriptorCollectionProvider actionDescriptorCollectionProvider) : base(null)
        {
            _actionDescriptorCollectionProvider = actionDescriptorCollectionProvider;
            _routesLogic = new RoutesLogic();
        }

        [HttpGet]
        public List<Endpoint> Index()
        {
            var list = _actionDescriptorCollectionProvider.ActionDescriptors.Items
                .Where(x => !IsObjectNull(x.AttributeRouteInfo))
                .Where(x => !x.AttributeRouteInfo.Template.Equals(""))
                .Where(x => !x.AttributeRouteInfo.Template.StartsWith('_'))
                .Where(x => !_routesLogic.IsMethodTypeUndefined(x))
                .Select(ad => new Endpoint
                {
                    method = _routesLogic.GetMethodType(ad),
                    path = _routesLogic.GetActionPath(ad),
                    name = _routesLogic.GetActionName(ad),
                    description = _routesLogic.GetDescription(ad)
                }).ToList();

            return list;
        }

        private bool IsObjectNull(object obj)
        {
            if (obj == null)
                return true;
            return false;
        }
    }
}
