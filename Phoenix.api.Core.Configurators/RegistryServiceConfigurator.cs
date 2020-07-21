using System;
using Phoenix.Api.Core.Configurators.Interfaces;

namespace Phoenix.Api.Core.Configurators
{
    public class RegistryServiceConfigurator : IRegistryServiceConfigurator
    {
        public IRabbitExchangeConfigurator RegistryRabbitConfig { get; set; } =
            new RabbitExchangeConfigurator
            {
                ExchangeName = "Dash.Service.Registry"
            };



        public bool RegisterWithController { get; set; } = false;
        public string RegisterRouteKey { get; set; } = "RegisterEndpoints";
        public string ServiceId { get; set; }
        public string Version { get; set; }
        public int NoTrafficTimerIntervalSeconds { get; set; } = 15;
        public string ServerUri { get; set; }

        public void Validate()
        {
            if (!RegisterWithController) return;
            if (RegistryRabbitConfig == null) throw new Exception("Missing RegistryRabbitConfig");
            if (string.IsNullOrWhiteSpace(ServiceId)) throw new Exception("ServiceId is required");
            if (string.IsNullOrWhiteSpace(Version)) throw new Exception("Version number is required");
            if (string.IsNullOrWhiteSpace(RegisterRouteKey)) throw new Exception("RegisterRoutKey is required");
            RegistryRabbitConfig.Validate();
        }
    }


}