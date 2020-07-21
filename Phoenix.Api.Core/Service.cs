using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Phoenix.Api.Core.Authorization;
using Phoenix.Api.Core.Bootstrappers;
using Phoenix.Api.Core.Logging.Interfaces;
using Phoenix.Api.Core.Models;

namespace Phoenix.Api.Core
{
    public static class Service
    {
        public static IEnumerable<IBootstrapper> Bootstrappers;
        public delegate void InjectionDelegate(IServiceCollection container);
        public static VersionInfo AssemblyVersion { get; } = new VersionInfo();
        public static dynamic Config { get; set; }
        public static IPhoenixLogger PhoenixLogger { get; set; }
        public static void Start(IEnumerable<IBootstrapper> bootstrappers = null)
        {
            Bootstrappers = bootstrappers;
            var parentFrame = new StackFrame(1, true);
            AssemblyVersion.ServiceVersion = parentFrame?.GetMethod()?.ReflectedType?.Assembly?.GetName()?.ToString() ?? "";
            AssemblyVersion.CoreVersion = typeof(Service).Assembly?.GetName()?.ToString() ?? "";
            var cert = HttpsCertificate.Load();
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(options =>
                {
                    options.AllowSynchronousIO = true;
                    options.Listen(IPAddress.Any, (int)Service.Config.ServiceConfiguration.HostPort, opts => opts.UseHttps(cert));
                })
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
