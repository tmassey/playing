using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Jose;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Phoenix.Api.Core.Authorization;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Models;
using Phoenix.Api.Core.Bootstrappers;
using Phoenix.Api.Core.Controllers.Attributes;
using Phoenix.Api.Core.Exceptions;

namespace Phoenix.Api.Core.Controllers
{
    [LogTraffic]
    [LogErrors]
    [RegistryReset]
    [Produces("application/json")]
    public class PhoenixBaseController : Controller
    {
        private readonly IUserManager _userManager;
        private IPhoenixUser _user;

        public PhoenixBaseController(IUserManager userManager)
        {
            _userManager = userManager;
        }

       public override void OnActionExecuting(ActionExecutingContext context)
        {
            var apiKey = context.HttpContext.Request.Query["ApiKey"];
            var jwtToken = context.HttpContext.Request.Query.ContainsKey("ApiKey")
                ? GetTokenFromApiKeyAsync(context.HttpContext.Request.Query["ApiKey"])
                : GetAuthJwtToken(context)
                ;
            try
            {
                var key = SigningCertificate.Load().GetRSAPrivateKey();
                var payload = JWT.Decode<JwtToken>(jwtToken.Replace("Bearer ", ""), key, JwsAlgorithm.RS256);
                var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
                _user = tokenExpires > DateTime.UtcNow ? _userManager.SetUser(payload, jwtToken) : null;
            }
            catch (Exception e)
            {
                LoggingBootstrapper.GetLogger().Fatal(e);
                _user = null;
            }
            
            base.OnActionExecuting(context);
        }

       private static string GetAuthJwtToken(ActionExecutingContext context)
       {
           if(context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            return context.HttpContext.Request.Headers["Authorization"][0];
           return null;
       }

       private  string GetTokenFromApiKeyAsync(string apiKey)
       {
           //var key = SigningCertificate.Load().GetRSAPrivateKey();
           //var client = _container.Resolve<IPhoenixHttpClient>();
           //var keys = (List<ApiKeyConfig>)Service.Config.ServiceConfiguration.ApiKeys;
           //var validApiKey = keys.FirstOrDefault(x => x.ApiKey == apiKey);
           //if (validApiKey == null)
           //    return null;
           //var decryptedApiKey = Encoding.UTF8.GetString(key.Decrypt(Convert.FromBase64String(validApiKey.EncryptedCredentials),
           //    RSAEncryptionPadding.OaepSHA512));
           //var userName = decryptedApiKey.Split(':')[0];
           //var password = decryptedApiKey.Split(':')[1];

           //var json = await Login(validApiKey, userName, password, client);

           //return json.access_token;
           return  "";
       }

        //private static async Task<dynamic> Login(ApiKeyConfig config, string userName, string userPassword, IPhoenixHttpClient client)
        //{
        //    var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
        //    {
        //        new KeyValuePair<string, string>("client_id", config.ClientId),
        //        new KeyValuePair<string, string>("client_secret", config.ClientSecret),
        //        new KeyValuePair<string, string>("redirect_uri", @"http://localhost:9100/callback"),
        //        new KeyValuePair<string, string>("grant_type", "password"),
        //        new KeyValuePair<string, string>("username", userName),
        //        new KeyValuePair<string, string>("password", userPassword)
        //    });

        //    var result =
        //        await client.PostAsync(Service.Config.ServiceConfiguration.IdentityServerUri + @"connect/token", content);
        //    var responseContent = await result.Content.ReadAsStringAsync();
        //    var json = JsonConvert.DeserializeObject<dynamic>(responseContent);
        //    return json;
        //}
        [ApiExplorerSettings(IgnoreApi = true)]
        public IPhoenixUser CurrentPhoenixUser()
        {
            return _user;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAuthentication()
        {
            if (CurrentPhoenixUser()?.Id == null && CurrentPhoenixUser()?.ClientId == null) throw new HttpUnauthorizedException();
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresScope(string scope)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().Scopes.All(s => s != scope))
                throw new HttpUnauthorizedException($"Missing scope {scope}.");
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAllScopes( params string[] scopes)
        {
            RequiresAuthentication();
            var userScopes = CurrentPhoenixUser().Scopes.ToList();
            if (scopes.Any(scope => !userScopes.Contains(scope)))
            {
                throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");
            }

            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAnyScope(params string[] scopes)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Scopes.Any(scopes.Contains))
                throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");

            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresRole(string role)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().Roles.All(s => s != role)) throw new HttpUnauthorizedException($"Missing role {role}.");
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAllRoles(params string[] roles)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Roles.All(roles.Contains)) throw new HttpUnauthorizedException($"Missing one or more role {string.Join(", ", roles)}.");
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAnyRole(params string[] roles)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Roles.Any(roles.Contains)) throw new HttpUnauthorizedException($"Missing all roles {string.Join(", ", roles)}.");
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresClient(string clientId)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().ClientId != clientId) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected {clientId}.");
            return true;
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        public  bool RequiresAnyClient(params string[] clientIds)
        {
            RequiresAuthentication();
            if (!clientIds.Contains(CurrentPhoenixUser().ClientId)) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected one of {string.Join(", ", clientIds)}.");
            return true;
        }
    }
}