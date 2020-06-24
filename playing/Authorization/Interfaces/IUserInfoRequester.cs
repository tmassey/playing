using playing.Authorization.Models;

namespace playing.Authorization.Interfaces
{
    public interface IUserInfoRequester
    {
        UserInfoDto Get(string token);
    }
}