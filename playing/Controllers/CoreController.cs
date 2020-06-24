using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using playing.Authorization.Interfaces;
using playing.Core.Extentions;

namespace playing.Controllers
{
    [Route("/")]
    [ApiController]
    public class CoreController : ControllerBase
    {
        [HttpGet]
        [Route("_ping")]
        public IActionResult Ping()
        {
            return Ok("pong");
        }
        [HttpGet]
        [Route("_version")]
        public IActionResult Version()
        {
            return Ok(Service.AssemblyVersion);
        }
        [HttpGet]
        [Route("_coffee")]
        public IActionResult Coffee()
        {
            return StatusCode(418, "I'm a little teapot short and stout!");
        }
    }

    public class BasePhoenixController : ControllerBase
    {
        private readonly IUserManager _userManager;

        public BasePhoenixController(IUserManager userManager)
        {
            _userManager = userManager;
            
        }
        //private IPhoenixUser Authenticate()
        //{
        //    var jwtToken = Request.Query.ContainsKey("ApiKey")
        //        ? GetTokenFromApiKeyAsync(Request.Query["ApiKey"]).Result
        //        : Request.Headers.FirstOrDefault(x=>x.Key=="Authorization").Value.ToString();
        //    try
        //    {
        //        var key = SigningCertificate.Load().GetRSAPrivateKey();
        //        var payload = JWT.Decode<JwtToken>(jwtToken.Replace("Bearer ", ""), key, JwsAlgorithm.RS256);
        //        var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
        //        if (tokenExpires > DateTime.UtcNow)
        //        {
                    
        //            return _userManager.SetUser(payload, jwtToken);
        //        }
        //        else
        //            return null;
        //    }
        //    catch (Exception e)
        //    {
        //        return null;
        //    }
        //}
        //private async Task<string> GetTokenFromApiKeyAsync(string apiKey)
        //{
        //    var key = SigningCertificate.Load().GetRSAPrivateKey();
        //    var client = _container.Resolve<IPhoenixHttpClient>();
        //    var keys = (List<ApiKeyConfig>)Service.Config.ServiceConfiguration.ApiKeys;
        //    var validApiKey = keys.FirstOrDefault(x => x.ApiKey == apiKey);
        //    if (validApiKey == null)
        //        return null;
        //    var decryptedApiKey = Encoding.UTF8.GetString(key.Decrypt(Convert.FromBase64String(validApiKey.EncryptedCredentials),
        //        RSAEncryptionPadding.OaepSHA512));
        //    var userName = decryptedApiKey.Split(':')[0];
        //    var password = decryptedApiKey.Split(':')[1];

        //    var json = await Login(validApiKey, userName, password, client);

        //    return json.access_token;
        //}

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
    }
}