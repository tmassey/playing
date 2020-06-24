using System.Timers;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.RabbitClient.Interfaces;
using playing.Configurators.Interfaces;
using playing.Core.Interfaces;
using playing.Core.Models;
using playing.Core.TimerFactory.Interfaces;

namespace playing.Core
{
    public class ReregistryService : IReregistryService
    {
        private readonly IRegistryServiceConfigurator _configurator;
        private readonly IPhoenixLogger _PhoenixLogger;
        private readonly IServiceTimer _timer;
        private readonly IDispatcher _dispatcher;
        private readonly IRabbitServer _rabbitServer;
        private bool _firstTime = true;


        public ReregistryService(IServiceTimer serviceTimer, IRabbitServer rabbitServer, IRegistryServiceConfigurator configurator, IPhoenixLogger PhoenixLogger)
        {
            _configurator = configurator;
            _PhoenixLogger = PhoenixLogger;
            _timer = serviceTimer;
            _timer.Init((_configurator.NoTrafficTimerIntervalSeconds * 1000), TimeoutReached);
            _rabbitServer = rabbitServer;
            _dispatcher = _rabbitServer.CreateFanoutExchangeDispatcher(_configurator.RegistryRabbitConfig.ExchangeName);             
        }

        public void Start()
        {
            _PhoenixLogger.Info("Registration service was started.");
            _timer.Start();
        }

        public void Stop()
        {
            _PhoenixLogger.Info("Registration service was stopped.");
            _timer.Stop();
        }

        public void Reset()
        {
            _timer.Stop();
            _timer.Start();
        }

        public void TimeoutReached(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();
            _PhoenixLogger.Info(_firstTime
                ? "Registering with controllers."
                : "Reregistering with controllers due to inactivity.");
            _dispatcher.Publish(_configurator.RegisterRouteKey, new RegisterRequest
            {
                base_uri = _configurator.ServerUri,
                end_point_uri = "/_routes",
                service_id = _configurator.ServiceId,
                version = _configurator.Version
            });
            _firstTime = false;
            _timer.Start();
        }

        public void Dispose()
        {
            _timer.Dispose();
            _dispatcher.Dispose();
           _rabbitServer.Dispose();
        }
    }
}