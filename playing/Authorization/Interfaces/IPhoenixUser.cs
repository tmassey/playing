using System;
using System.Collections.Generic;
using System.Security.Claims;
using Phoenix.Api.Core.Logging.Models;

namespace playing.Authorization.Interfaces
{
    public interface IPhoenixUser
    {
        IEnumerable<Claim> Claims { get; }
        Guid? Id { get; }
        List<string> EmailAddresses { get; }
        IEnumerable<string> Scopes { get; }
        IEnumerable<string> Roles { get; }
        string PolicyName { get; }
        string ClientId { get; }
        bool EmailVerified { get; }
        string UnverifiedEmailAddress { get; }
        string FamilyName { get; }
        string GivenName { get; }
        string Name { get; }
        string UserName { get; }

        int? MembershipId { get; }

        UserDetails GetUserDetails();
    }
}