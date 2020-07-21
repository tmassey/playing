using System;
using System.Diagnostics;
using System.Net;
using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.Api.Core.Bootstrappers;
using Phoenix.Api.Core.Logging.Models;

namespace Phoenix.Api.Core.Controllers.Attributes
{
    public class LogTrafficAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var watchKey = $"StartTime{context.HttpContext.TraceIdentifier}";
            if (!context.HttpContext.Items.ContainsKey(watchKey))
            {
                context.HttpContext.Items.Add(watchKey, Stopwatch.StartNew());
            }

            base.OnActionExecuting(context);
        }

        public override void OnResultExecuted(ResultExecutedContext context)
        {
            var watchKey = $"StartTime{context.HttpContext.TraceIdentifier}";
            if (context.HttpContext.Items.ContainsKey(watchKey))
            {
                context.HttpContext.Items.TryGetValue(watchKey, out var watch);
                var stopWatch = (Stopwatch)watch;
                if (stopWatch != null)
                {
                    stopWatch.Stop();
                    var requestTime = Convert.ToInt32(stopWatch.Elapsed.TotalMilliseconds);
                    LogNetworkCall(context, requestTime);
                    return;
                }
            }
            LogNetworkCall(context);
        }

        private static void LogNetworkCall(ResultExecutedContext context, int duration = 0)
        {
            LoggingBootstrapper.GetLogger().Network(context.HttpContext.Request.Method,
                (HttpStatusCode)context.HttpContext.Response.StatusCode, context.HttpContext.Request.Path, duration,
                Service.Config.ServiceConfiguration?.ServiceId, GetUserDetails((PhoenixBaseController)context.Controller));
        }

        private static UserDetails GetUserDetails(PhoenixBaseController controller)
        {
            var user = controller.CurrentPhoenixUser();
            return user?.GetUserDetails();
        }
    }
}
