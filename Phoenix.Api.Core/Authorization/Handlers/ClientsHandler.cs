using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Phoenix.Api.Core.Authorization.Requirements;

namespace Phoenix.Api.Core.Authorization.Handlers
{
    public class ClientsHandler : AuthorizationHandler<ClientsRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ClientsRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "client_id"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else
            {
                var name = context.User.Claims.First(c => c.Type == "client_id");
                var client = name.Value;
                if(!requirement.Clients.Any(s=>s.Contains(client)))
                    context.Fail();
                else
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}