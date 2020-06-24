using System;
using System.Collections.Generic;
using System.Security.Claims;
using Phoenix.Api.Core.Logging.Models;
using playing.Authorization.Interfaces;

namespace playing.Authorization
{
    public class PhoenixUser : ClaimsPrincipal, IPhoenixUser
    {
        private readonly IUserManager _userManager;

        public PhoenixUser(IUserManager userManager)
        {
            _userManager = userManager;
        }

        public Guid? Id => _userManager.IdentityId;
        public IEnumerable<Claim> Claims => _userManager.Claims;
        public IEnumerable<string> Roles => _userManager.Roles;
        public string PolicyName => _userManager.PolicyName;
        public IEnumerable<string> Scopes => _userManager.Scopes;
        public List<string> EmailAddresses => _userManager.GetEmailAddresses();
        public bool EmailVerified => _userManager.GetEmailAddressVerified();
        public int? MembershipId => _userManager.MembershipId;
        public string ClientId => _userManager.ClientId;
        public string FamilyName => _userManager.GetFamilyName();
        public string GivenName => _userManager.GetGivenName();
        public string Name => _userManager.GetName();
        public string UnverifiedEmailAddress => _userManager.GetUnverifiedEmailAddress();

        public string UserName => _userManager.GetUserName();

        public UserDetails GetUserDetails()
        {
            return new UserDetails
            {
                ClientId = ClientId,
                Id = Id,
                UserName = UserName,
                Email = EmailAddresses[0]
            };
        }

        private static Guid? ParseGuid(string value)
        {
            Guid guid;
            if (Guid.TryParse(value, out guid)) return guid;
            return null;
        }

        private static bool ParseBool(string value)
        {
            bool boolVal;
            return bool.TryParse(value, out boolVal) && boolVal;
        }
    }
}