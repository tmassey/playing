namespace Phoenix.Api.Core.Configurators.Models
{
    public class EndpointConfiguration
    {
        public string ShareExchangeIntegrationsBaseUrl { get; set; }
        public string IdentityBaseUrl { get; set; }
        public string CrmIntegrationsBaseUrl { get; set; }
        public string NotificationsIntegrationsBaseUrl { get; set; }
        public string SignaturesBaseUrl { get; set; }
    }
}