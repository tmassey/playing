using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using playing.Authorization.Interfaces;
using playing.Core.Exceptions;

namespace playing.Core.Extentions
{
    public static class PhoenixSecurityExtensions
    {
        public static IPhoenixUser CurrentPhoenixUser(this ControllerBase module)
        {
            return module.User as IPhoenixUser;
        }

        public static bool RequiresAuthentication(this ControllerBase module)
        {
            if (CurrentPhoenixUser(module)?.Id == null && CurrentPhoenixUser(module)?.ClientId == null) throw new HttpUnauthorizedException();
            return true;
        }

        public static bool RequiresScope(this ControllerBase module, string scope)
        {
            RequiresAuthentication(module);
            if (CurrentPhoenixUser(module).Scopes.All(s => s != scope))
                module.Unauthorized($"Missing scope {scope}.");
                //throw new HttpUnauthorizedException($"Missing scope {scope}.");
            return true;
        }

        public static bool RequiresAllScopes(this ControllerBase module, params string[] scopes)
        {
            RequiresAuthentication(module);
            var userScopes = CurrentPhoenixUser(module).Scopes;
            foreach (var scope in scopes)
            {
                if (!userScopes.Contains(scope))
                    throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");
            }

            return true;
        }

        public static bool RequiresAnyScope(this ControllerBase module, params string[] scopes)
        {
            RequiresAuthentication(module);
            if (!CurrentPhoenixUser(module).Scopes.Any(scopes.Contains))
                throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");

            return true;
        }

        public static bool RequiresRole(this ControllerBase module, string role)
        {
            RequiresAuthentication(module);
            if (CurrentPhoenixUser(module).Roles.All(s => s != role)) throw new HttpUnauthorizedException( $"Missing role {role}.");
            return true;
        }

        public static bool RequiresAllRoles(this ControllerBase module, params string[] roles)
        {
            RequiresAuthentication(module);
            if (!CurrentPhoenixUser(module).Roles.All(roles.Contains)) throw new HttpUnauthorizedException( $"Missing one or more role {string.Join(", ", roles)}." );
            return true;
        }

        public static bool RequiresAnyRole(this ControllerBase module, params string[] roles)
        {
            RequiresAuthentication(module);
            if (!CurrentPhoenixUser(module).Roles.Any(roles.Contains)) throw new HttpUnauthorizedException($"Missing all roles {string.Join(", ", roles)}." );
            return true;
        }

        public static bool RequiresClient(this ControllerBase module, string clientId)
        {
            RequiresAuthentication(module);
            if (CurrentPhoenixUser(module).ClientId != clientId) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected {clientId}.");
            return true;
        }

        public static bool RequiresAnyClient(this ControllerBase module, params string[] clientIds)
        {
            RequiresAuthentication(module);
            if (!clientIds.Contains(CurrentPhoenixUser(module).ClientId)) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected one of {string.Join(", ", clientIds)}.");
            return true;
        }
    }
}