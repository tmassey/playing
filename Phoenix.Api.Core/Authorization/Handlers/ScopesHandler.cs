using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Phoenix.Api.Core.Authorization.Requirements;

namespace Phoenix.Api.Core.Authorization.Handlers
{
    public class ScopesHandler : AuthorizationHandler<ScopesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            ScopesRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "scope"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else
            {
                var scopes = context.User.FindAll(c => c.Type == "scope").Select(x=>x.Value).ToList();
                if (!requirement.Scopes.Any(s => scopes.Contains(s)))
                        context.Fail();
                else
                    context.Succeed(requirement);
            }
           
            return Task.CompletedTask;
        }
    }
}
