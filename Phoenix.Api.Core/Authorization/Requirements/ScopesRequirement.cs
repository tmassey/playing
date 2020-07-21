using Microsoft.AspNetCore.Authorization;

namespace Phoenix.Api.Core.Authorization.Requirements
{
    public class ScopesRequirement : IAuthorizationRequirement
    {
        public string[] Scopes { get; }

        public ScopesRequirement(string[] scopes)
        {
            Scopes = scopes;
        }
    }
}
