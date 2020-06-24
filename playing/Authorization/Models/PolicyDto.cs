using System.Collections.Generic;

namespace playing.Authorization.Models
{
    public class PolicyDto
    {
        public int? MembershipId { get; set; }
        public string PolicyName { get; set; }
        public List<string> Roles { get; set; }
    }
}