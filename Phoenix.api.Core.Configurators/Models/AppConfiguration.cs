using Phoenix.Api.Core.Configurators.Interfaces;

namespace Phoenix.Api.Core.Configurators.Models
{
    public class AppConfiguration : IAppConfiguration
    {
        public ServiceConfiguration ServiceConfiguration { get; set; }
        public string ConnectionString { get; set; }
        public EndpointConfiguration EndpointConfiguration { get; set; }
    }
}