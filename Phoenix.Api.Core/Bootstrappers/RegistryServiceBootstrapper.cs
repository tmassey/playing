using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.Configurators;
using Phoenix.Api.Core.RegistryService.Interfaces;
using Phoenix.Api.Core.TimerFactory;
using Phoenix.Api.Core.TimerFactory.Interfaces;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class RegistryServiceBootstrapper
    {
        private static IRegistryService _service;
        public static IRegistryService GetRegistryService()
        {
            return _service ??= new RegistryService.RegistryService(new ServiceTimer(),
                RabbitBootstrapper.GetRabbitServer(),
                new RegistryServiceConfigurator
                {
                    NoTrafficTimerIntervalSeconds = Service.Config.ServiceConfiguration.ControllerConfiguration.MSTimeout,
                    RegisterWithController =
                        Service.Config.ServiceConfiguration.ControllerConfiguration.RegisterWithController,
                    ServiceId = Service.Config.ServiceConfiguration.ServiceId,
                    Version = Service.Config.ServiceConfiguration.ServiceVersion,
                    ServerUri = Service.Config.ServiceConfiguration.ServiceUri,
                    RegistryRabbitConfig = new RabbitExchangeConfigurator
                    {
                        ExchangeName = Service.Config.ServiceConfiguration.ControllerConfiguration.ControllerExchange,
                        HostName = Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitHostName,
                        Password = Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitPassword,
                        Port = Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitPort,
                        UserName = Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitUserName
                    }
                },
                LoggingBootstrapper.GetLogger());
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<IServiceTimer, ServiceTimer>();
            services.AddSingleton(GetRegistryService());
        }
        public static void Configure(IApplicationBuilder app)
        {
            if (!Service.Config.ServiceConfiguration.ControllerConfiguration.RegisterWithController) return;
            GetRegistryService().TimeoutReached(null, null);
        }
    }
}