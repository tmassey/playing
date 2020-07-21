using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Phoenix.Api.Core.Authorization;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Configurators.Models;

namespace Phoenix.Api.Core.Extensions
{
    public static class PhoenixSecurityExtensions
    {
        public static IPhoenixUser CurrentPhoenixUser(this ControllerBase module)
        {
            return module.User as IPhoenixUser;
        }
        public static string GetAuthJwtToken(this HttpContext context)
        {
            if (context.Request.Headers.ContainsKey("Authorization"))
                return context.Request.Headers["Authorization"][0];
            return null;
        }

        public static string GetTokenFromApiKeyAsync(this HttpContext context)
        {
            var apiKey = context.Request.Query["ApiKey"];
            var key = SigningCertificate.Load().GetRSAPrivateKey();
            var client = (HttpClient)context.RequestServices.GetService(typeof(HttpClient));
            var keys = (List<ApiKeyConfig>)Service.Config.ServiceConfiguration.ApiKeys;
            var validApiKey = keys.FirstOrDefault(x => x.ApiKey == apiKey);
            if (validApiKey == null)
                return null;
            var decryptedApiKey = Encoding.UTF8.GetString(key.Decrypt(Convert.FromBase64String(validApiKey.EncryptedCredentials),
                RSAEncryptionPadding.OaepSHA512));
            var userName = decryptedApiKey.Split(':')[0];
            var password = decryptedApiKey.Split(':')[1];

            var json = Login(validApiKey, userName, password, client).Result;

            return json?.access_token;
        }

        private static async Task<dynamic> Login(ApiKeyConfig config, string userName, string userPassword, HttpClient client)
        {
            var content = new FormUrlEncodedContent(new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", config.ClientId),
                new KeyValuePair<string, string>("client_secret", config.ClientSecret),
                new KeyValuePair<string, string>("redirect_uri", @"http://localhost:9100/callback"),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username", userName),
                new KeyValuePair<string, string>("password", userPassword)
            });

            var result =
                await client.PostAsync(Service.Config.ServiceConfiguration.IdentityServerUri + @"connect/token", content);
            var responseContent = await result.Content.ReadAsStringAsync();
            var json = JsonConvert.DeserializeObject<dynamic>(responseContent);
            return json;
        }
    }
}