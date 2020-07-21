using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.Api.Core.Bootstrappers;
using Phoenix.Api.Core.Logging.Models;

namespace Phoenix.Api.Core.Controllers.Attributes
{
    public class LogErrorsAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception !=null)
                LoggingBootstrapper.GetLogger().Fatal(context.Exception,GetUserDetails((PhoenixBaseController)context.Controller));
            base.OnActionExecuted(context);
        }
        
        private static UserDetails GetUserDetails(PhoenixBaseController controller)
        {
            var user = controller.CurrentPhoenixUser();
            return user?.GetUserDetails();
        }
    }
}