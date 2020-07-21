using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.Api.Core.Bootstrappers;
using Phoenix.Api.Core.Controllers;
using Phoenix.Api.Core.Routes;
using Serilog;

namespace Phoenix.Api.Core
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env)
        {
            ConfigurationBootstrapper.LoadConfiguration(env, null);
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            RabbitBootstrapper.ConfigureServices(services);
            LoggingBootstrapper.ConfigureServices(services);
            DocumentationBootstrapper.ConfigureServices(services);
            services.AddWebEncoders();
            services.AddDataProtection();
            AuthenticationBootstrapper.ConfigureServices(services);
            RegistryServiceBootstrapper.ConfigureServices(services);
            services.AddMvcCore(opt => opt.EnableEndpointRouting = false);
            services.AddControllers()
                .AddNewtonsoftJson(GetSerializerSettings);
            ConfigureServicesForBootstrappers(services);
        }

        private static void ConfigureServicesForBootstrappers(IServiceCollection services)
        {
            if (Service.Bootstrappers != null)
                foreach (var bootstrapper in Service.Bootstrappers)
                {
                    bootstrapper.ConfigureServices(services);
                }
        }

        private static void GetSerializerSettings(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.Formatting = Formatting.Indented;
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            LoggingBootstrapper.Configure(app);
            app.UseRouting();
            AuthenticationBootstrapper.Configure(app);
            DocumentationBootstrapper.Configure(app);
            RegistryServiceBootstrapper.Configure(app);
            ConfigureForBootstrappers(app);

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }

        private static void ConfigureForBootstrappers(IApplicationBuilder app)
        {
            if (Service.Bootstrappers != null)
                foreach (var bootstrapper in Service.Bootstrappers)
                {
                    bootstrapper.Configure(app);
                }
        }
    }
}