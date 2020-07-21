namespace Phoenix.Api.Core.Models
{
    public class RegisterRequest
    {
        public string service_id { get; set; } 
        public string end_point_uri { get; set; } 
        public string base_uri { get; set; }
        public string version { get; set; }
    }
}
