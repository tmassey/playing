using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace playing.Core.Attributes
{
    public class ScopeAttribute : AuthorizeAttribute
    {
        private readonly string[] _scopes;

        public ScopeAttribute(params string[] scopes)
        {
            _scopes = scopes;
        }


        //protected override bool IsAuthorized(HttpActionContext actionContext)
        //{

        //    var user = actionContext.RequestContext.Principal as ClaimsPrincipal;

        //    if (user == null || !user.Identity.IsAuthenticated) return false;

        //    var claims = new ClaimsParser(user);

        //    if (!_scopes.Any(s => claims.Scopes.Contains(s))) return false;
        //    return base.IsAuthorized(actionContext);
        //}

    }
}
