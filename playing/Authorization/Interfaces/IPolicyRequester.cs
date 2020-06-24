using playing.Authorization.Models;

namespace playing.Authorization.Interfaces
{
    public interface IPolicyRequester
    {
        PolicyDto GetAsync(string userId);
        

    }
}