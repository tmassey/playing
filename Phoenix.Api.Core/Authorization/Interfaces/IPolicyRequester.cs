using Phoenix.Api.Core.Authorization.Models;

namespace Phoenix.Api.Core.Authorization.Interfaces
{
    public interface IPolicyRequester
    {
        PolicyDto GetAsync(string userId);
        

    }
}