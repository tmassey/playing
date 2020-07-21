using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Policy names map to scopes
            //IEnumerable<AuthorizeAttribute> requiredScopes = context.MethodInfo
            //    .GetCustomAttributes(true)
            //    .OfType<AuthorizeAttribute>()
            //    .Distinct().ToList();

            //requiredScopes.Union(
            //    context.MethodInfo?.DeclaringType?.GetCustomAttributes(true).OfType<AuthorizeAttribute>()
            //        .Distinct().ToList());

            //if (requiredScopes.Any())
            //{
                operation.Responses.Add("401", new OpenApiResponse { Description = "Unauthorized" });
                operation.Responses.Add("403", new OpenApiResponse { Description = "Forbidden" });

                var oAuthScheme = new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "oauth2" }
                };

                operation.Security = new List<OpenApiSecurityRequirement>
                {
                    new OpenApiSecurityRequirement
                    {
                        [ oAuthScheme ] = new List<string>{"Scope2"}
                    }
                };
            //}
        }
    }
}