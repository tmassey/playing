using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Models;

namespace Phoenix.Api.Core.Authorization
{
    public class UserManager : IUserManager
    {
        private readonly IPolicyRequester _policyRequester;
        private readonly IUserInfoRequester _userInfoRequester;
        private Lazy<UserInfoDto> _userInfo;
        private JwtToken _token;
        private string _rawToken;
        private Guid _id;
        public Guid? IdentityId { get; set; }
        public string ClientId { get; set; }
        public int? MembershipId { get; set; }
        private Lazy<UserInfoDto> UserInfo => _userInfo ??= new Lazy<UserInfoDto>(_userInfoRequester.Get(_rawToken));
        public Dictionary<string, string> UserProperties { get; set; }
        public string PolicyName { get; set; }
        public IEnumerable<string> Scopes { get; set; }
        public IEnumerable<Claim> Claims { get; set; }
        public List<string> Roles { get; set; }
        public UserManager(IPolicyRequester policyRequester, IUserInfoRequester userInfoRequester)
        {
            _policyRequester = policyRequester;
            _userInfoRequester = userInfoRequester;
            UserProperties= new Dictionary<string, string>();
        }

        public IPhoenixUser SetUser(JwtToken token, string rawToken)
        {
            _token = token;
            _rawToken = rawToken;
            ClientId = _token.client_id;
            if (token.sub != null)
            {
                Guid.TryParse(token.sub,out _id);
                if (_id == new Guid())
                    IdentityId = null;
                else
                    IdentityId = _id;
            }

            if (token.sub != null)
            {
                Claims = new ClaimsPrincipal(new HttpListenerBasicIdentity(token.sub, null)).Claims;
                
                Roles = token.role.ToList();
            }
            
            //var policy = _policyRequester.GetAsync(IdentityId.ToString());
            //if (policy != null)
            //{
            //    MembershipId = policy.MembershipId;
            //    Roles = policy.Roles;
            //    PolicyName = policy.PolicyName;
            //}

            Scopes = token.scope;
            return new PhoenixUser(this);
        }

        

        public List<string> GetEmailAddresses()
        {
            if (UserInfo.Value == null || UserInfo.Value.email == null)
                return new List<string> { "UNKNOWN@SMCHCN.NET" };
            if (UserInfo.Value.email is string ||
                UserInfo.Value.email.Type != Newtonsoft.Json.Linq.JTokenType.Array)
                return new List<string> { UserInfo.Value.email };
            var items = new List<string>();
            foreach (var item in UserInfo.Value.email)
                items.Add(item.ToString());
            return items;
        }
        public string GetUserName()
        {
            return UserInfo?.Value?.preferred_username ?? "UNKNOWN";
        }

        public string GetFamilyName()
        {
            return UserInfo?.Value?.family_name ?? "UNKNOWN";
        }
        public string GetGivenName()
        {
            return UserInfo?.Value?.given_name ?? "UNKNOWN";
        }
        public string GetName()
        {
            return UserInfo?.Value?.name ?? "UNKNOWN";
        }

        public string GetUnverifiedEmailAddress()
        {
            return UserInfo?.Value?.unverified_email ?? "UNKNOWN";
        }
        public bool GetEmailAddressVerified()
        {
            return UserInfo?.Value?.email_verified[1] ?? false;
        }


    }
}
