using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.Logging;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.Logging.Models;
using playing.Configurators;
using playing.Configurators.Interfaces;

namespace playing
{
    public static class Service
    {
        public delegate void InjectionDelegate(IServiceCollection container);
        public static VersionInfo AssemblyVersion { get; private set; } = new VersionInfo();
        public static dynamic Config { get; set; }
        public static IServiceConfigurator Configuration;
        public static IPhoenixLogger PhoenixLogger { get; set; }
        public static bool AllowShutdown = true;
        public static void Start()
        {
            var parentFrame = new StackFrame(1, true);
            AssemblyVersion.ServiceVersion = parentFrame?.GetMethod()?.ReflectedType?.Assembly?.GetName()?.ToString() ?? "";
            AssemblyVersion.CoreVersion = typeof(Service).Assembly?.GetName()?.ToString() ?? "";

        }
        private static IServiceConfigurator BuildConfiguration(InjectionDelegate setupDi)
        {
            return new ServiceConfigurator
            {
                AutoConfig = false,
                ServiceUri = Config.ServiceConfiguration.ServiceUri,
                ServiceName = Config.ServiceConfiguration.ServiceName,
                ServiceDescription = Config.ServiceConfiguration.ServiceDescription,

                UseAuthentication = true, //Enable authentication with Phoenix identity server
                IdentityServerUri = Config.ServiceConfiguration.IdentityServerUri,
                LogToConsole = Config.ServiceConfiguration.Logging.LogToConsole, //If true, log entries are dumped to the console, otherwise they are pushed to Rabbit
                LogLevel = Severity.Network,
                LogRabbitConfig =
                   new RabbitExchangeConfigurator //Rabbit server settings for logging, ignored if LogToConsole = true
                   {
                       ExchangeName = Config.ServiceConfiguration.Logging.LoggingExchange,
                       Port = Config.ServiceConfiguration.Logging.LoggingPort,
                       HostName = Config.ServiceConfiguration.Logging.LoggingHost,
                       UserName = Config.ServiceConfiguration.Logging.LoggingUserName,
                       Password = Config.ServiceConfiguration.Logging.LoggingPassword
                   },
                RegistryConfig = new RegistryServiceConfigurator //Settings for talking to the microservice controller.
                {
                    RegisterWithController = Config.ServiceConfiguration.ControllerConfiguration.RegisterWithController, //Weather or not to register with the controller, if false all other registry settings are ignored
                    NoTrafficTimerIntervalSeconds = Config.ServiceConfiguration.ControllerConfiguration.MSTimeout, //Amount of time to wait to reregister with the controller if no traffic has been received.
                    RegisterRouteKey = "RegisterEndpoints", //Routing key to use when registering with a controller. (Most likely this will not change)
                    RegistryRabbitConfig = new RabbitExchangeConfigurator //Rabbit server settings for registering with the controller
                    {
                        ExchangeName = Config.ServiceConfiguration.ControllerConfiguration.ControllerExchange, //This will usually not change
                        Port = Config.ServiceConfiguration.ControllerConfiguration.RabbitPort,
                        HostName = Config.ServiceConfiguration.ControllerConfiguration.RabbitHostName,
                        UserName = Config.ServiceConfiguration.ControllerConfiguration.RabbitUserName,
                        Password = Config.ServiceConfiguration.ControllerConfiguration.RabbitPassword
                    },
                    ServiceId = Config.ServiceConfiguration.ServiceId,  //This is the unique id of the services, this is used buy the controller to help prevent duplicate routes
                    Version = Config.ServiceConfiguration.ServiceVersion //The version of the service. This is appended by the controller, used for running multiple versions of the same service.
                },
                InjectionDelegate = setupDi //A delegate that will be called during setup, this allows you to set up any DI you need           
            };
        }

        private static void ValidateConfig(IServiceConfigurator config)
        {
            try
            {
                config.Validate();
            }
            catch (Exception ex)
            {
                var cw = new ConsoleWriter();
                var message = $"Services configuration failed with: {ex.Message}";
                cw.WriteLineInColor(message, ConsoleColor.White, ConsoleColor.Red);
                Debug.WriteLine(message);
                if (AllowShutdown) Environment.Exit(100);
                throw;
            }

        }
    }
}
