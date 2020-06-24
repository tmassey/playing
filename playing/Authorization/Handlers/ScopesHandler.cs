using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using playing.Authorization.Requirements;
using playing.Controllers;

namespace playing.Authorization.Handlers
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
    public class RolesHandler : AuthorizationHandler<RolesRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            RolesRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "sub"))
            {
                context.Fail();
                return Task.CompletedTask;
            }
            else
            {
                var resource = (RouteEndpoint)context.Resource;
                var name = context.User.Claims;
                //if (!requirement.Roles.Any(s => resource.CurrentPhoenixUser().Roles.Contains(s)))
                //    context.Fail();
                //else
                //    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
