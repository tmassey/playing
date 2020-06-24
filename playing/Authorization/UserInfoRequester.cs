using playing.Authorization.Interfaces;
using playing.Authorization.Models;
using RestSharp;

namespace playing.Authorization
{
    public class UserInfoRequester : IUserInfoRequester
    {
        private RestSharp.RestClient _client;

        public UserInfoRequester()
        {
            //_client = new RestSharp.RestClient(Service.Config.ServiceConfiguration.IdentityServerUri);
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