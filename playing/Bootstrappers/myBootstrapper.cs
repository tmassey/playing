using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Phoenix.Api.Core.Bootstrappers;
using playing.Controllers;

namespace playing.Bootstrappers
{
    public class MyBootstrapper:BaseBootstrapper
    {
        public override void ConfigureServices(IServiceCollection services)
        {
            //This is where you would register your injections
            var assembly = typeof(ValuesController).GetTypeInfo().Assembly;
            // This creates an AssemblyPart, but does not create any related parts for items such as views.
            var part = new AssemblyPart(assembly);
            services.AddControllersWithViews()
                .ConfigureApplicationPartManager(apm => apm.ApplicationParts.Add(part));
        }
        private static void GetSerializerSettings(MvcNewtonsoftJsonOptions options)
        {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
            options.SerializerSettings.Formatting = Formatting.Indented;
        }
        public override void Configure(IApplicationBuilder app)
        {
            //This happens last before running
            //This is where you can turn on the services you have registered
        }
    }
}
