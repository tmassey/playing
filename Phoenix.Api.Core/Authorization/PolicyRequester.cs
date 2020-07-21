using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Models;
using RestSharp;

namespace Phoenix.Api.Core.Authorization
{
    public class PolicyRequester : IPolicyRequester
    {
        private readonly RestClient _client;

        public PolicyRequester()
        {
            _client = new RestClient(Service.Config.ServiceConfiguration.IdentityServerUri);
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