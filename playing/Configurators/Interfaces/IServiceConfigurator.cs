using System.Collections.Generic;
using Phoenix.Api.Core.Logging.Models;

namespace playing.Configurators.Interfaces
{
    public interface IServiceConfigurator
    {
        bool AutoConfig { get; set; }
        string ServiceUri { get; set; }
        string ServiceName { get; set; }
        IList<string> AlternatUrls { get; set; }
        string ServiceDescription { get; set; }
        bool UseAuthentication { get; set; }
        string IdentityServerUri { get; set; }
        Service.TinyIocInjectionDelegate InjectionDelegate { get; set; }
        //Startup.AppBuilderInjectionDelegate AppBuilderDelegate { get; set; }
        IRegistryServiceConfigurator RegistryConfig { get; set; }
        bool LogToConsole { get; set; }
        Severity LogLevel { get; set; }
        IRabbitExchangeConfigurator LogRabbitConfig { get; set; }
        string DiagnosticsPassword { get; set; }       
        void Validate();
    }
}