using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using playing.Authorization;
using playing.Authorization.Interfaces;

namespace playing.Core
{
    public class Startup
    {
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
                .AddCookie()
                .AddOpenIdConnect("oidc", options =>
                {
                    options.SignInScheme = "Cookies";
                    options.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;
                    options.RequireHttpsMetadata = true;
                    options.ClientId = Service.Config.ServiceConfiguration.IdentityClient;
                    options.ClientSecret = Service.Config.ServiceConfiguration.IdentityClientSecret;
                    options.ResponseType = "code id_token";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.SaveTokens = true;
                });
            
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseOwin();
        }
    }
}