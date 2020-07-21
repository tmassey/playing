using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.RabbitClient;
using Phoenix.Api.Core.RabbitClient.Interfaces;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class RabbitBootstrapper
    {
        private static IRabbitServer _rabbitServer;
        
        public static IRabbitServer GetRabbitServer()
        {
            return _rabbitServer ??= new RabbitServer(
                Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitHostName,
                Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitUserName,
                Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitPassword,
                Service.Config.ServiceConfiguration.ControllerConfiguration.RabbitPort);
        }

        public static  void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(GetRabbitServer());
        }
    }
}