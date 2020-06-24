using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
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

        
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDocumentation(services);
            services.AddMvc(opt=>opt.EnableEndpointRouting=false);
            services.AddWebEncoders();
            services.AddDataProtection();
            ConfigureAuthentication(services);
            services.AddControllers();
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://identity.sandbox.samaritanministries.org/";
                    options.RequireHttpsMetadata = false;
                    //options.Events.OnTokenValidated = OnTokenValidated;
                    options.Audience = "api1";
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("PLayingPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // custom requirements
                });
            });
        }

        private static void ConfigureDocumentation(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "My API", Version = "v1"});
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://identity.sandbox.samaritanministries.org/connect/authorize",
                                UriKind.Absolute),
                            Scopes = new Dictionary<string, string>
                            {
                                {"readAccess", "Access read operations"},
                                {"writeAccess", "Access write operations"}
                            }
                        }
                    }
                });
            });
        }

        private Task OnTokenValidated(TokenValidatedContext arg)
        {
            var tmp = arg.Principal;
            return Task.CompletedTask;

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            LoggerFactory.Create(Opt => Opt.AddConsole());
            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseSwagger();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            //app.UseMvc();
           
        }
    }
}
