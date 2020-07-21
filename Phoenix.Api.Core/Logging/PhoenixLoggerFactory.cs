using Phoenix.Api.Core.Logging.ConsoleLogger;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.RabbitClient;

namespace Phoenix.Api.Core.Logging
{
    public static class PhoenixLoggerFactory
    {
  
        public static IPhoenixLogger CreateLogger(Configurators.Models.Logging config)
        {
            return config.LogToConsole ? CreateConsoleLogger(config) : CreateRabbitLogger(config);
        }

        private static ConsoleLogger.ConsoleLogger CreateConsoleLogger(Configurators.Models.Logging config)
        {
           return new ConsoleLogger.ConsoleLogger(new ConsoleWriter()) {LogLevel = config.LogLevel};
        }

        private static IPhoenixLogger CreateRabbitLogger(Configurators.Models.Logging config)
        {
            var server = new RabbitServer(config.LoggingHost, config.LoggingUserName, config.LoggingPassword, config.LoggingPort);
            return new RabbitLogger.RabbitLogger(server, config.LoggingExchange) { LogLevel = config.LogLevel };
        }
    }
}
