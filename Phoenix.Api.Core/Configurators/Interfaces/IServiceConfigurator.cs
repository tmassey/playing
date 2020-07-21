using Phoenix.Api.Core.Logging.Enums;

namespace Phoenix.Api.Core.Configurators.Interfaces
{
    public interface IServiceConfigurator
    {
        bool AutoConfig { get; set; }
        string ServiceUri { get; set; }
        string ServiceName { get; set; }
        string ServiceDescription { get; set; }
        bool UseAuthentication { get; set; }
        string IdentityServerUri { get; set; }
        Service.InjectionDelegate InjectionDelegate { get; set; }
        //Startup.AppBuilderInjectionDelegate AppBuilderDelegate { get; set; }
        IRegistryServiceConfigurator RegistryConfig { get; set; }
        bool LogToConsole { get; set; }
        Severity LogLevel { get; set; }
        IRabbitExchangeConfigurator LogRabbitConfig { get; set; }
        string DiagnosticsPassword { get; set; }       
        void Validate();
    }
}