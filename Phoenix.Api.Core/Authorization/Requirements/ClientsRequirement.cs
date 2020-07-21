using Microsoft.AspNetCore.Authorization;

namespace Phoenix.Api.Core.Authorization.Requirements
{
    public class ClientsRequirement : IAuthorizationRequirement
    {
        public string[] Clients { get; }

        public ClientsRequirement(string[] clients)
        {
            Clients = clients;
        }
    }
}