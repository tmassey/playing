using System.ComponentModel;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Routing;
using Phoenix.Api.Core.Controllers;

namespace Phoenix.Api.Core.Routes
{
    public class RoutesLogic : IRoutesLogic
    {
       

        public bool IsMethodTypeUndefined(ActionDescriptor ad)
        {
            return ad.EndpointMetadata.FirstOrDefault(x => x is HttpMethodMetadata)==null;
        }
        public string GetMethodType(ActionDescriptor ad)
        {
            foreach (var item in ad.EndpointMetadata) 
                if(item is HttpMethodMetadata metadata)
                    return metadata.HttpMethods[0];
            return "UNKNOWN";
        }

        public string GetDescription(ActionDescriptor ad)
        {
            foreach (var item in ad.EndpointMetadata)
                if (item is DescriptionAttribute attribute)
                    return attribute.Description;
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