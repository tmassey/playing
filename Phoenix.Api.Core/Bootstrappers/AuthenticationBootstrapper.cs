using System;
using System.Diagnostics;
using System.Net;
using System.Threading.Tasks;
using IdentityModel.AspNetCore.AccessTokenManagement;
using IdentityModel.Client;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.Authorization;
using Phoenix.Api.Core.Authorization.Handlers;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Requirements;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class AuthenticationBootstrapper 
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient("client")
                .AddHttpMessageHandler<UserAccessTokenHandler>();
            services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
                .AddIdentityServerAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme,
                    jwtOptions =>
                    {
                        jwtOptions.Authority = Service.Config.ServiceConfiguration.IdentityServerUri;
                        jwtOptions.Audience = Service.Config.ServiceConfiguration.IdentityClient;
                        jwtOptions.SaveToken=true;
                        jwtOptions.Events = new JwtBearerEvents
                        {
                            OnMessageReceived = context => OnMessageReceived(context),
                            OnAuthenticationFailed = context => OnAuthenticationFailed(context),
                        };
                    },
                    referenceOptions => { referenceOptions.ClientCredentialStyle = ClientCredentialStyle.PostBody; });
           
            services.AddAuthorization(BuildPolicies);
            RegisterAdditionalAuthenticationServices(services);
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseAuthorization();
        }
        private static void RegisterAdditionalAuthenticationServices(IServiceCollection services)
        {
            services.AddSingleton<IAuthorizationHandler, ScopesHandler>();
            services.AddSingleton<IAuthorizationHandler, RolesHandler>();
            services.AddSingleton<IAuthorizationHandler, ClientsHandler>();
            services.AddTransient<IPolicyRequester, PolicyRequester>();
            services.AddTransient<IUserInfoRequester, UserInfoRequester>();
            services.AddTransient<IUserManager, UserManager>();
        }

        private static Task OnMessageReceived(MessageReceivedContext context)
        {
            var watchKey = $"StartTime{context.HttpContext.TraceIdentifier}";
            if (!context.HttpContext.Items.ContainsKey(watchKey))
            {
                context.HttpContext.Items.Add(watchKey, Stopwatch.StartNew());
            }

            return Task.CompletedTask;
        }

        private static Task OnAuthenticationFailed(AuthenticationFailedContext context)
        {
            var watchKey = $"StartTime{context.HttpContext.TraceIdentifier}";
            if (context.HttpContext.Items.ContainsKey(watchKey))
            {
                context.HttpContext.Items.TryGetValue(watchKey, out var watch);
                var stopWatch = (Stopwatch)watch;
                if (stopWatch != null)
                {
                    stopWatch.Stop();
                    var requestTime = Convert.ToInt32(stopWatch.Elapsed.TotalMilliseconds);
                    LogUnauthorizedNetworkCall(context.HttpContext, requestTime);
                    return Task.CompletedTask;
                }
            }
            LogUnauthorizedNetworkCall(context.HttpContext);
            return Task.CompletedTask;
        }

        private static void LogUnauthorizedNetworkCall(HttpContext context, int duration = 0)
        {
            LoggingBootstrapper.GetLogger().Network(context.Request.Method,
                HttpStatusCode.Unauthorized, context.Request.Path, duration,
                Service.Config.ServiceConfiguration?.ServiceId);
        }

        private static void BuildPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("PlayingPolicy", policy =>
            {
                policy.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme);
                policy.RequireAuthenticatedUser();
                policy.Requirements.Add(new ScopesRequirement(new[] {"scope2"}));
                policy.Requirements.Add(new RolesRequirement(new[] {"Read"}));
                policy.Requirements.Add(new ClientsRequirement(new[] { "interactive" }));
            });
        }
    }
}