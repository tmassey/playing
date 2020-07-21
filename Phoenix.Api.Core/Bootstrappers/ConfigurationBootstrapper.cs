using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Phoenix.Api.Core.Configurators.Models;
using Phoenix.Api.Core.Extensions;

namespace Phoenix.Api.Core.Bootstrappers
{
    public static class ConfigurationBootstrapper
    {
        public static void LoadConfiguration(IWebHostEnvironment env, IConfigurationRoot configuration)
        {
            if (configuration == null)
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(env.ContentRootPath)
                    .AddJsonFile("appsettings.json", false, true);
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                //.AddEnvironmentVariables();
                configuration = builder.Build();
                var fullConfig = new AppConfiguration();
                configuration.Bind(fullConfig);

                Service.Config = fullConfig.ToDynamic();
            }
        }
    }
}