using playing.Configurators.Interfaces;

namespace playing.Configurators.Models
{
    public class AppConfiguration : IAppConfiguration
    {
        public ServiceConfiguration ServiceConfiguration { get; set; }
        public string ConnectionString { get; set; }
        public EndpointConfiguration EndpointConfiguration { get; set; }
    }
}