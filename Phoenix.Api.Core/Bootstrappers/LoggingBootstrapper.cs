using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Jose;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.ApiResponces;
using Phoenix.Api.Core.Authorization;
using Phoenix.Api.Core.Authorization.Interfaces;
using Phoenix.Api.Core.Authorization.Models;
using Phoenix.Api.Core.Errors;
using Phoenix.Api.Core.Exceptions;
using Phoenix.Api.Core.Extensions;
using Phoenix.Api.Core.Logging;
using Phoenix.Api.Core.Logging.ConsoleLogger;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.Logging.Models;
using Phoenix.Api.Core.Logging.Sinks.RabbitMq;
using Serilog;
using Serilog.Events;
using Serilog.Formatting.Json;
using Serilog.Sinks.RabbitMQ;
using Serilog.Sinks.SystemConsole.Themes;

namespace Phoenix.Api.Core.Bootstrappers
{
    public class LoggingBootstrapper
    { 
        private static IPhoenixLogger _logger;
        private static HttpClient _httpClient;
        private static IUserManager _userManager;

        public static IPhoenixLogger GetLogger()
        {
            return _logger ??= PhoenixLoggerFactory.CreateLogger(Service.Config.ServiceConfiguration.Logging);
        }
        public static void ConfigureServices(IServiceCollection services)
        {
            if (Service.Config.ServiceConfiguration.Logging.LogToConsole)
            {
                ConfigureConsoleLogger(services);
            }
            else
                ConfigureRabbitMqSink();
            services.AddSingleton(GetLogger());
        }

        private static void ConfigureConsoleLogger(IServiceCollection services)
        {
            services.AddSingleton<IConsoleWriter, ConsoleWriter>();
            services.AddSingleton<IPhoenixLogger, ConsoleLogger>();
        }

        private static void ConfigureRabbitMqSink()
        {
            var config = new RabbitMQConfiguration
            {
                Hostname = Service.Config.ServiceConfiguration.Logging.LoggingHost,
                Username = Service.Config.ServiceConfiguration.Logging.LoggingUserName,
                Password = Service.Config.ServiceConfiguration.Logging.LoggingPassword,
                Exchange = Service.Config.ServiceConfiguration.Logging.LoggingExchange,
                ExchangeType = "topic",
                RouteKey = "#",
                DeliveryMode = RabbitMQDeliveryMode.NonDurable,
                Port = Service.Config.ServiceConfiguration.Logging.LoggingPort
            };
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)
                .MinimumLevel.Override("System", LogEventLevel.Verbose)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .WriteTo.RabbitMQ(config, new JsonFormatter())
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Code)
                .CreateLogger();
        }

        public static void Configure(IApplicationBuilder app)
        {
            _httpClient = app.ApplicationServices.GetService<HttpClient>();
            _userManager = app.ApplicationServices.GetService<IUserManager>();
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Run(async context =>
                {
                    context.Response.StatusCode = 500;
                    context.Response.ContentType = "application/json";
                    var errorContext = context.Features.Get<IExceptionHandlerFeature>();
                    await context.Response.WriteAsync(Newtonsoft.Json.JsonConvert.SerializeObject(await HandleException(errorContext.Error, context)));
                });
            });
        }
        private static async Task<ApiResponse<object>> HandleException(Exception ex, HttpContext context)
        {
            return ex switch
            {
                ValidationException vex => await HandleValidationException(vex, context),
                HttpException nex => await HandleHttpException(nex, context),
                _ => await HandleUnexpectedException(ex, context)
            };
        }
        private static UserDetails GetUserDetails(HttpContext context)
        {
            var apiKey = context.Request.Query["ApiKey"];
            var jwtToken = context.Request.Query.ContainsKey("ApiKey")
                    ? context.GetTokenFromApiKeyAsync()
                    : context.GetAuthJwtToken()
                ;
            try
            {
                var key = SigningCertificate.Load().GetRSAPrivateKey();
                var payload = JWT.Decode<JwtToken>(jwtToken.Replace("Bearer ", ""), key, JwsAlgorithm.RS256);
                var tokenExpires = DateTimeOffset.FromUnixTimeSeconds(payload.exp);
                var user = tokenExpires > DateTime.UtcNow ? _userManager.SetUser(payload, jwtToken) : null;
                return user?.GetUserDetails();
            }
            catch (Exception e)
            {
                Service.PhoenixLogger.Fatal(e);
            }

            return null;
        }

        private static int GetResponseTime(HttpContext context)
        {
            var watchKey = $"StartTime{context.TraceIdentifier}";
            if (context.Items.ContainsKey(watchKey))
            {
                context.Items.TryGetValue(watchKey, out var watch);
                var stopWatch = (Stopwatch)watch;
                if (stopWatch != null)
                {
                    stopWatch.Stop();
                    var requestTime = Convert.ToInt32(stopWatch.Elapsed.TotalMilliseconds);
                    return requestTime;
                }
            }

            return 0;
        }
        private static async Task<ApiResponse<object>> HandleValidationException(ValidationException ex, HttpContext context)
        {
            
            GetLogger().Network(context.Request?.Method, HttpStatusCode.BadRequest, context.Request?.Path,
                GetResponseTime(context), Service.Config.ServiceConfiguration?.ServiceId, GetUserDetails(context));
            
            
            var validationErrors = ex.ValidationResult.MemberNames.Select(
                e => new ValidationError(
                    "500", 
                    ex.ValidationResult.ErrorMessage, 
                    e, 
                    "UNKNOWN"))
                .Cast<Error>()
                .ToList();

            return new ApiResponse<object>
            {
                Errors = validationErrors
            };
        }

        private static async Task<ApiResponse<object>> HandleHttpException(HttpException ex, HttpContext context)
        {
            GetLogger().Network(context.Request?.Method, ex.HttpStatus, context.Request?.Path,
                GetResponseTime(context), Service.Config.ServiceConfiguration?.ServiceId, GetUserDetails(context));
            
            if ((int)ex.HttpStatus < 500)
            {
                return new ApiResponse<object>
                {
                    Errors = new List<Error>
                    {
                        new Error(ex.HttpStatus.ToString(), ex.Message)
                    }
                };
            }

            return new ApiResponse<object>
            {
                Errors = new List<Error>
                {
                    new FatalError(ex.HttpStatus.ToString(), ex.Message, ex.StackTrace)
                }
            };
        }

        private static async Task<ApiResponse<object>> HandleUnexpectedException(Exception ex, HttpContext context)
        {
            GetLogger().Network(context.Request?.Method, 
                HttpStatusCode.InternalServerError, 
                context.Request?.Path,
                GetResponseTime(context), 
                Service.Config.ServiceConfiguration?.ServiceId, 
                GetUserDetails(context));

            GetLogger().Fatal(ex, GetUserDetails(context));

            var error = new FatalError(HttpStatusCode.InternalServerError.ToString(), ex.Message, ex.StackTrace);
            return new ApiResponse<dynamic> { Errors = new List<Error> { error } };
        }
    }
}