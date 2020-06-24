using Microsoft.AspNetCore.Authorization;

namespace playing.Authorization.Requirements
{
    public class ScopesRequirement : IAuthorizationRequirement
    {
        public string[] Scopes { get; }

        public ScopesRequirement(string[] scopes)
        {
            Scopes = scopes;
        }
    }
    public class RolesRequirement : IAuthorizationRequirement
    {
        public string[] Roles { get; }

        public RolesRequirement(string[] roles)
        {
            Roles = roles;
        }
    }
}
