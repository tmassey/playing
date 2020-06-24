using playing.Configurators.Models;

namespace playing.Configurators.Interfaces
{
    public interface IAppConfiguration
    {
        ServiceConfiguration ServiceConfiguration { get; set; }
    }
}