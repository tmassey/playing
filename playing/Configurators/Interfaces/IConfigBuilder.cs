using Microsoft.Extensions.Configuration;

namespace playing.Configurators.Interfaces
{
    public interface IConfigBuilder
    {
        IConfiguration LoadConfiguration();
    }
}