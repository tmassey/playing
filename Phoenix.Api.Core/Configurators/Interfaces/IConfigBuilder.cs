using Microsoft.Extensions.Configuration;

namespace Phoenix.Api.Core.Configurators.Interfaces
{
    public interface IConfigBuilder
    {
        IConfiguration LoadConfiguration();
    }
}