namespace playing.Configurators.Models
{
    public class SecureClient
    {
        public string OAuthClient { get; set; }
        public string OAuthSecret { get; set; }
        public string AuthSrvUrl { get; set; }
        public string BaseUrl { get; set; }
    }
}