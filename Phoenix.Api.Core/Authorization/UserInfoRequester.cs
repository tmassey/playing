using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Models;
using RestSharp;

namespace Phoenix.Api.Core.Authorization
{
    public class UserInfoRequester : IUserInfoRequester
    {
        private readonly RestClient _client;

        public UserInfoRequester()
        {
            _client = new RestClient(Service.Config.ServiceConfiguration.IdentityServerUri);
        }
        
        public UserInfoDto Get(string token)
        {
            var request = new RestRequest("connect/userinfo");
            request.AddHeader("Authorization", token);
            var result = _client.GetAsync<UserInfoDto>(request);

            result.Wait();
            return result.IsCompletedSuccessfully ? result.Result : null;
        }
    }
}