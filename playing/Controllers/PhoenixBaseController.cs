using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using playing.Authorization;
using playing.Authorization.Interfaces;
using playing.Authorization.Models;
using playing.Configurators.Models;
using playing.Core.Exceptions;

namespace playing.Controllers
{
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
                ? GetTokenFromApiKeyAsync(context.HttpContext.Request.Query["ApiKey"]).Result
                : context.HttpContext.Request.Headers["Authorization"][0].ToString()
                ;
            try
            {
                var key = SigningCertificate.Load().GetRSAPrivateKey();
                var payload = JWT.Decode<JwtToken>(jwtToken.Replace("Bearer ", ""), key, JwsAlgorithm.RS256);
                var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
                if (tokenExpires > DateTime.UtcNow)
                {
                    _user = _userManager.SetUser(payload, jwtToken);
                }
                else
                    _user = null;
            }
            catch (Exception e)
            {
                _user = null;
            }
            
            base.OnActionExecuting(context);
        }
       private async Task<string> GetTokenFromApiKeyAsync(string apiKey)
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
           return "";
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
        public IPhoenixUser CurrentPhoenixUser()
        {
            return _user;
        }

        public  bool RequiresAuthentication()
        {
            if (CurrentPhoenixUser()?.Id == null && CurrentPhoenixUser()?.ClientId == null) throw new HttpUnauthorizedException();
            return true;
        }

        public  bool RequiresScope(string scope)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().Scopes.All(s => s != scope))
                throw new HttpUnauthorizedException($"Missing scope {scope}.");
            return true;
        }

        public  bool RequiresAllScopes( params string[] scopes)
        {
            RequiresAuthentication();
            var userScopes = CurrentPhoenixUser().Scopes;
            foreach (var scope in scopes)
            {
                if (!userScopes.Contains(scope))
                    throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");
            }

            return true;
        }

        public  bool RequiresAnyScope(params string[] scopes)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Scopes.Any(scopes.Contains))
                throw new HttpUnauthorizedException($"Missing all scopes {string.Join(", ", scopes)}.");

            return true;
        }

        public  bool RequiresRole(string role)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().Roles.All(s => s != role)) throw new HttpUnauthorizedException($"Missing role {role}.");
            return true;
        }

        public  bool RequiresAllRoles(params string[] roles)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Roles.All(roles.Contains)) throw new HttpUnauthorizedException($"Missing one or more role {string.Join(", ", roles)}.");
            return true;
        }

        public  bool RequiresAnyRole(params string[] roles)
        {
            RequiresAuthentication();
            if (!CurrentPhoenixUser().Roles.Any(roles.Contains)) throw new HttpUnauthorizedException($"Missing all roles {string.Join(", ", roles)}.");
            return true;
        }

        public  bool RequiresClient(string clientId)
        {
            RequiresAuthentication();
            if (CurrentPhoenixUser().ClientId != clientId) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected {clientId}.");
            return true;
        }

        public  bool RequiresAnyClient(params string[] clientIds)
        {
            RequiresAuthentication();
            if (!clientIds.Contains(CurrentPhoenixUser().ClientId)) throw new HttpUnauthorizedException($"Mismatched Client Id. Expected one of {string.Join(", ", clientIds)}.");
            return true;
        }
    }
}