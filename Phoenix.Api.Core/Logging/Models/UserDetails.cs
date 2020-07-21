using System;

namespace Phoenix.Api.Core.Logging.Models
{
    public class UserDetails
    {
        public Guid? Id { get; set; }
        public string Email { get; set; }
        public string ClientId { get; set; }
        public string UserName { get; set; }
    }
}
