using playing.Authorization.Interfaces;
using playing.Authorization.Models;
using RestSharp;

namespace playing.Authorization
{
    public class PolicyRequester : IPolicyRequester
    {
        private RestSharp.RestClient _client;

        public PolicyRequester()
        {
            _client = new RestSharp.RestClient(Service.Config.ServiceConfiguration.IdentityServerUri);
        }

        public PolicyDto GetAsync(string userId)
        {
            PolicyDto policy = null;
            var request = new RestRequest("/Identity/" + userId, DataFormat.Json);
            var result = _client.GetAsync<PolicyDto>(request);
            result.Wait();
            if (result.IsCompletedSuccessfully)
                policy = result.Result;            
            return policy;
        }
    }
}