using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using playing.Controllers;

namespace playing.Core.Routes
{
    public interface IRoutesLogic
    {
        string GetMethodType(ActionDescriptor ad);
        string GetDescription(ActionDescriptor ad);
        string GetActionPath(ActionDescriptor ad);
        string GetActionName(ActionDescriptor ad);
    }

    public class RoutesLogic : IRoutesLogic
    {
        private RoutesController _routesController;

        public RoutesLogic(RoutesController routesController)
        {
            _routesController = routesController;
        }

        public string GetMethodType(ActionDescriptor ad)
        {
            foreach (var item in ad.EndpointMetadata) 
                if(item is HttpMethodMetadata)
                    return ((HttpMethodMetadata)item).HttpMethods[0];
            return "UNKNOWN";
        }

        public string GetDescription(ActionDescriptor ad)
        {
            foreach (var item in ad.EndpointMetadata)
                if (item is DescriptionAttribute)
                    return ((DescriptionAttribute)item).Description;
            return "";
        }

        public string GetActionPath(ActionDescriptor ad)
        {
            return ad.AttributeRouteInfo.Template;
        }

        public string GetActionName(ActionDescriptor ad)
        {
            string response = "";
            response += GetMethodType(ad) + "_" + ad.RouteValues["controller"] + "_" + ad.RouteValues["action"];
            foreach (var pram in ad.Parameters)
            {
                if (pram.BindingInfo?.BindingSource?.DisplayName == null && pram.ParameterType != typeof(ActionDescriptor))
                    response += "_" + pram.Name;
            }
            return response;
        }
    }
}