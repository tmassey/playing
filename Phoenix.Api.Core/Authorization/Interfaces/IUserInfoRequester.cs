using Phoenix.Api.Core.Authorization.Models;

namespace Phoenix.Api.Core.Authorization.Interfaces
{
    public interface IUserInfoRequester
    {
        UserInfoDto Get(string token);
    }
}