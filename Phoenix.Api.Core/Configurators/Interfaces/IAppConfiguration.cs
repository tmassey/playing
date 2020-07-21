using Phoenix.Api.Core.Configurators.Models;

namespace Phoenix.Api.Core.Configurators.Interfaces
{
    public interface IAppConfiguration
    {
        ServiceConfiguration ServiceConfiguration { get; set; }
    }
}