using Microsoft.AspNetCore.Mvc.Abstractions;

namespace Phoenix.Api.Core.Routes
{
    public interface IRoutesLogic
    {
        bool IsMethodTypeUndefined(ActionDescriptor ad);
        string GetMethodType(ActionDescriptor ad);
        string GetDescription(ActionDescriptor ad);
        string GetActionPath(ActionDescriptor ad);
        string GetActionName(ActionDescriptor ad);
    }
}