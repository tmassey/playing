namespace Phoenix.Api.Core.Configurators.Interfaces
{
    public interface IRegistryServiceConfigurator
    {
        IRabbitExchangeConfigurator RegistryRabbitConfig { get; set; }
        bool RegisterWithController { get; set; }
        string                     RegisterRouteKey       { get; set; }
        string                     ServiceId              { get; set; }
        string                     Version                { get; set; }
        int                        NoTrafficTimerIntervalSeconds { get; set; }
        string                     ServerUri              { get; set; }
        void Validate();
    }
}