using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Phoenix.Api.Core.Authorization.Requirements;

namespace Phoenix.Api.Core.Authorization.Handlers
{
    public class RolesHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesRequirement requirement)
        {
            var roles = context.User.Claims.Where(x => x.Type == "http://schemas.microsoft.com/ws/2008/06/identity/claims/role").Select(x => x.Value).ToList();
            if (!roles.Any()) return Task.CompletedTask;
            if (!requirement.Roles.Any(s => roles.Contains(s)))
                context.Fail();
            else
                context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}