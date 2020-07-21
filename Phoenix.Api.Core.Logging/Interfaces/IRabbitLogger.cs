using Phoenix.Api.Core.RabbitClient.Models;

namespace Phoenix.Api.Core.Logging.Interfaces
{
    public interface IRabbitLogger
    {
        string HostName { get; set; }
        string ExchangeName { get; set; }
        string RoutingKey { get; set; }
        int Port { get; set; }
        DispatchType DispatchType { get; set; }
    }
}
