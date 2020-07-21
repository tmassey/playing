using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class DocumentationBootstrapper
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Phoenix.Api.Core", Version = "v1"});
                c.EnableAnnotations();
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Password = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri("https://tmassey.mooo.com:5001/connect/authorize", UriKind.Absolute),
                            TokenUrl = new Uri("https://tmassey.mooo.com:5001/connect/token", UriKind.Absolute),
                            Scopes = new Dictionary<string, string>()
                            {
                                { "openid","Openid"},
                                { "profile","Profile access"},
                                { "offline_access","Refresh Tokens"},
                                { "scope2","Scope for my service"}
                            }
                        }
                    }
                });
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
        }

        public static void Configure(IApplicationBuilder app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DefaultModelExpandDepth(2);
                c.DefaultModelRendering(ModelRendering.Model);
                c.DefaultModelsExpandDepth(3);
                c.DisplayOperationId();
                c.DisplayRequestDuration();
                c.DocExpansion(DocExpansion.None);
                c.EnableDeepLinking();
                c.EnableFilter();
                c.MaxDisplayedTags(5);
                c.ShowExtensions();
                c.ShowCommonExtensions();
                c.EnableValidator();
                c.SupportedSubmitMethods(SubmitMethod.Get, SubmitMethod.Head);
                c.OAuthUsePkce();
                c.OAuthClientId("interactive");
                c.OAuthClientSecret("49C1A7E1-0C79-4A89-A3D6-A37998FB86B0");

                c.SwaggerEndpoint("/_moduledocs", "My API V1");
            });
        }
    }
}