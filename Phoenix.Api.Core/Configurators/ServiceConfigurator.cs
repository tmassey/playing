using System;
using Phoenix.Api.Core.Configurators.Interfaces;
using Phoenix.Api.Core.Logging.Enums;

namespace Phoenix.Api.Core.Configurators
{
    public class ServiceConfigurator : IServiceConfigurator
    {
        private string _serviceUri;
        private IRegistryServiceConfigurator _registryServiceConfigurator = new RegistryServiceConfigurator();

        public bool AutoConfig { get; set; } = false;
        public string ServiceUri {
            get => _serviceUri;
            set
            {
                _serviceUri = value;
                if (RegistryConfig != null) RegistryConfig.ServerUri = value;
            }
        }
        public string ServiceName { get; set; }
        public string ServiceDescription { get; set; }
        public bool UseAuthentication { get; set; } = false;
        public string IdentityServerUri { get; set; }
        public Service.InjectionDelegate InjectionDelegate { get; set; }

        public IRegistryServiceConfigurator RegistryConfig {
            get => _registryServiceConfigurator;
            set
            {
                _registryServiceConfigurator = value;
                if(value != null)_registryServiceConfigurator.ServerUri = _serviceUri;
            }
        }

        public bool LogToConsole { get; set; } = false;
        public Severity LogLevel { get; set; } = Severity.Info;
        public IRabbitExchangeConfigurator LogRabbitConfig { get; set; } = new RabbitExchangeConfigurator
        {
            ExchangeName = "Logs"
        };

        public string DiagnosticsPassword { get; set; }


        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(ServiceUri) && AutoConfig == false) throw new Exception("ServerUri is required when AutoConfig is false.");
            if (string.IsNullOrWhiteSpace(ServiceName)) throw new Exception("ServiceName is required");
            if (string.IsNullOrWhiteSpace(ServiceDescription)) throw new Exception("ServiceDescription is required");
            if (UseAuthentication && string.IsNullOrWhiteSpace(IdentityServerUri)) throw new Exception("IdentityServerUri is required when UseAuthentication is true");
            if (RegistryConfig == null) throw new Exception("RegistryConfig is required.");
            RegistryConfig.Validate();
            if (!LogToConsole && LogRabbitConfig == null) throw new Exception("LogRabbitConfig is required when LogToConsole is false");
            if (!LogToConsole) LogRabbitConfig.Validate();
        }


    }

}
