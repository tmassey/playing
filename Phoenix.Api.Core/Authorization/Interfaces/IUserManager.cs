using System;
using System.Collections.Generic;
using System.Security.Claims;
using Phoenix.Api.Core.Authorization.Models;

namespace Phoenix.Api.Core.Authorization.Interfaces
{
    public interface IUserManager
    {
        Guid? IdentityId { get; set; }
        string ClientId { get; set; }
        int? MembershipId { get; set; }
        Dictionary<string, string> UserProperties { get; set; }
        IEnumerable<Claim> Claims { get; set; }
        List<string> Roles { get; set; }
        string PolicyName { get; set; }
        IEnumerable<string> Scopes { get; set; }
        IPhoenixUser SetUser(JwtToken token, string rawToken);
        List<string> GetEmailAddresses();
        string GetUserName();
        string GetFamilyName();
        string GetGivenName();
        string GetName();
        string GetUnverifiedEmailAddress();
        bool GetEmailAddressVerified();
    }
}