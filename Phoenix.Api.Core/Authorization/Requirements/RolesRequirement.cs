using Microsoft.AspNetCore.Authorization;

namespace Phoenix.Api.Core.Authorization.Requirements
{
    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; }

        public RolesRequirement(string[] roles)
        {
            Roles = roles;
        }
    }
}