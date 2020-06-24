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
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.OpenApi.Models;
using playing.Authorization;
using playing.Authorization.Handlers;
using playing.Authorization.Interfaces;
using playing.Authorization.Requirements;
using playing.Configurators.Models;
using playing.Controllers;
using playing.Core.Extentions;
using playing.Core.Routes;

namespace playing
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                //.AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                //.AddEnvironmentVariables();
            Configuration = builder.Build();
            var fullConfig = new AppConfiguration();
            ConfigurationBinder.Bind(Configuration, fullConfig);
            
            Service.Config = fullConfig.ToDynamic();
            Service.Start();
        }

        public IConfigurationRoot Configuration { get; }

        
        public void ConfigureServices(IServiceCollection services)
        {
            ConfigureDocumentation(services);
            services.AddMvc(opt=>opt.EnableEndpointRouting=false);
            services.AddWebEncoders();
            services.AddDataProtection();
            ConfigureAuthentication(services);

            services.AddTransient<IPolicyRequester, PolicyRequester>();
            services.AddTransient<IUserInfoRequester, UserInfoRequester>();
            services.AddTransient<IUserManager, UserManager>();

            services.AddControllers();

            
        }

        private static void ConfigureAuthentication(IServiceCollection services)
        {
            //services.AddAuthentication("Bearer")
            //    .AddJwtBearer("Bearer", options =>
            //    {
            //        options.Authority = "https://identity.sandbox.samaritanministries.org/";
            //        options.RequireHttpsMetadata = true;
            //        options.Configuration.TokenEndpoint = 
            //        //options.Events.OnTokenValidated = OnTokenValidated;
            //        //options.Audience = "api1";
            //    });
            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                    {
                        options.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;
                        options.RequireHttpsMetadata = false;
                        //options.Configuration = new OpenIdConnectConfiguration
                        //{
                        //    TokenEndpoint = Service.Config.ServiceConfiguration.IdentityServerUri + "/connect/token",
                        //    AuthorizationEndpoint =
                        //        Service.Config.ServiceConfiguration.IdentityServerUri + "/connect/authorize",
                        //    UserInfoEndpoint = Service.Config.ServiceConfiguration.IdentityServerUri +
                        //                       "/connect/userinfo",
                        //    JwksUri = Service.Config.ServiceConfiguration.IdentityServerUri +
                        //              "/.well-known/openid-configuration/jwks"
                        //};
                        //options.Events.OnTokenValidated = OnTokenValidated;
                        options.Audience = "api1";
                    });
                //.AddIdentityServerAuthentication(options =>
                //{
                //    // base-address of your identityserver
                //    options.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;

                //    // name of the API resource
                //    options.ApiName = Service.Config.ServiceConfiguration.IdentityClient;
                //    options.ApiSecret = Service.Config.ServiceConfiguration.IdentityClientSecret;
                //    options.EnableCaching = true;
                //    options.CacheDuration = TimeSpan.FromMinutes(10); // that's the default
                //})
                //.AddCookie()
                //.AddOpenIdConnect("oidc", options =>
                //{
                //    options.SignInScheme = "Cookies";
                //    options.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;
                //    options.RequireHttpsMetadata = false;
                //    options.ClientId = Service.Config.ServiceConfiguration.IdentityClient;
                //    options.ClientSecret = Service.Config.ServiceConfiguration.IdentityClientSecret;
                //    options.ResponseType = "code id_token";
                //    options.GetClaimsFromUserInfoEndpoint = true;
                //    options.SaveTokens = true;
                //});
            services.AddAuthorization(options =>
            {
                options.AddPolicy("PlayingPolicy", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    // custom requirements
                });
                options.AddPolicy("AtLeast21", policy =>
                {
                    policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                    policy.RequireAuthenticatedUser();
                    policy.Requirements.Add(new ScopesRequirement(new string[] {"memberships"}));
                    policy.Requirements.Add(new RolesRequirement(new string[] { "memberships" }));
                });
            });
            services.AddSingleton<IAuthorizationHandler, ScopesHandler>();
            services.AddSingleton<IAuthorizationHandler, RolesHandler>();

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
