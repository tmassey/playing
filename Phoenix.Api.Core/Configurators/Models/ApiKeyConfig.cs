namespace Phoenix.Api.Core.Configurators.Models
{
    public class ApiKeyConfig
    {
        public string ApiKey { get; set; }
        public string EncryptedCredentials { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}