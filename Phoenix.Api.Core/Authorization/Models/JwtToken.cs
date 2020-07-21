using System.Collections.Generic;

namespace Phoenix.Api.Core.Authorization.Models
{
    public class JwtToken
    {
        public long exp;
        public string sub;
        public IEnumerable<string> scope { get; set; }
        public IEnumerable<string> role { get; set; }
        public IEnumerable<string> amr { get; set; }
        public string client_id { get; set; }
    }
}