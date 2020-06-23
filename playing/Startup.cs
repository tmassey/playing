using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using playing.Controllers;
using playing.Core.Routes;

namespace playing
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            Service.Start();
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvcCore()
                .AddAuthorization();
            services.AddWebEncoders();
            services.AddDataProtection();

            services.AddAuthentication(config =>
                {
                    config.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                    config.DefaultChallengeScheme = "oidc";
                })
                .AddCookie();
                //.AddOpenIdConnect("oidc", options =>
                //{
                //    options.SignInScheme = "Cookies";
                //    options.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;
                //    options.RequireHttpsMetadata = true;
                //    options.ClientId = Service.Config.ServiceConfiguration.IdentityClient;
                //    options.ClientSecret = Service.Config.ServiceConfiguration.IdentityClientSecret;
                //    options.ResponseType = "code id_token";
                //    options.GetClaimsFromUserInfoEndpoint = true;
                //    options.SaveTokens = true;
                //});
            
            services.AddTransient<IRoutesLogic, RoutesLogic>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            LoggerFactory.Create(Opt => Opt.AddConsole());
           

            app.UseMvc();
            
        }
    }
}
