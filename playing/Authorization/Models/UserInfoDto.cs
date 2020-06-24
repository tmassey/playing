namespace playing.Authorization.Models
{
    public class UserInfoDto
    {
        public string sub { get; set; }
        public string name { get; set; }
        public string given_name { get; set; }
        public string family_name { get; set; }
        public dynamic email { get; set; }
        public string website { get; set; }
        public string unverified_email { get; set; }
        public string preferred_username { get; set; }
        public dynamic email_verified { get; set; }
    }
}