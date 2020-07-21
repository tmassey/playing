using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.Api.Core.Bootstrappers;

namespace Phoenix.Api.Core.Controllers.Attributes
{
    public class RegistryResetAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            RegistryServiceBootstrapper.GetRegistryService().Reset();
            base.OnActionExecuting(context);
        }
    }
}