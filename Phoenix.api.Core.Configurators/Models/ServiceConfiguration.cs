using System.Collections.Generic;

namespace Phoenix.Api.Core.Configurators.Models
{
    public class ServiceConfiguration
    {
        public string ServiceUri { get; set; }
        public int HostPort { get; set; }
        public string BindingCertificateThumbprint { get; set; }
        public string ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public string ServiceVersion { get; set; }
        public string IdentityServerUri { get; set; }
        public string IdentityClient { get; set; }
        public string IdentityClientSecret { get; set; }
        public string IdentityEnvironment { get; set; }
        public Logging Logging { get; set; }
        public ControllerConfiguration ControllerConfiguration { get; set; }
        public Authorization Authorization { get; set; }
        public Dictionary<string,SecureClient> SecureClients { get; set; }
        public List<ApiKeyConfig> ApiKeys { get; set; }
    }
}