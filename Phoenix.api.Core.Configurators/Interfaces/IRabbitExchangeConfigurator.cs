namespace Phoenix.Api.Core.Configurators.Interfaces
{
    public interface IRabbitExchangeConfigurator
    {
        string ExchangeName { get; set; }
        string HostName { get; set; }
        string UserName { get; set; }
        string Password { get; set; }
        int Port { get; set; }
        void Validate();
    }
}