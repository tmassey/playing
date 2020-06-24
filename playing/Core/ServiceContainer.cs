using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Phoenix.Api.Core.Logging.Interfaces;
using playing.Configurators.Interfaces;
using playing.Core.Interfaces;

namespace playing.Core
{
    public class ServiceContainer : IServiceContainer
    {
        private IDisposable _host;
        protected bool AllowShutdown = true;
        protected IServiceConfigurator Config { get; set; } = Service.Configuration;
        protected IPhoenixLogger Logger { get; set; } = Service.PhoenixLogger;

        public void Start()
        {
            var serverUri = Config.ServiceUri;
            TryAndStartServer(serverUri);
        }

        public void Stop()
        {
            _host.Dispose();
            if (AllowShutdown) Environment.Exit(0);
        }

        private void TryAndStartServer(string serverUri)
        {
            try
            {
                StartServer(serverUri);
            }
            catch (Exception ex)
            {
                var message = $"The services failed to start: {ex.Message}";
                while (ex.InnerException != null)
                {
                    message += $"\r\n --> {ex.InnerException.Message}";
                    ex = ex.InnerException;
                }

                Logger.Error(message);
                Debug.WriteLine(message);
                if (AllowShutdown) Environment.Exit(100);
                throw;
            }
        }
        private static X509Certificate2 LoadCertificate()
        {
            X509Certificate2Collection certs;

            using (var store = new X509Store(StoreName.My, StoreLocation.LocalMachine))
            {
                store.Open(OpenFlags.ReadOnly);
                certs = store.Certificates.Find(X509FindType.FindByThumbprint,
                    Service.Config.ServiceConfiguration.BindingCertificateThumbprint, true);
                store.Close();
            }

            if (certs.Count == 0) throw new Exception($"Could not load the ssl cert with thumbprint {Service.Config.ServiceConfiguration.BindingCertificateThumbprint}");
            return certs[0];
        }

        public virtual void StartServer(string serverUri)
        {
            if (Service.Config.ServiceConfiguration.ServiceUri.Contains("localhost"))
                SetupLocalhostService();
            else
                SetupSSLService();
        }

        private static void SetupSSLService()
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(options =>
                {
                    options.AllowSynchronousIO = true;
                    options.Listen(IPAddress.Any, (int) Service.Config.ServiceConfiguration.HostPort, opts =>
                        {
                            var cert = LoadCertificate();
                            opts.UseHttps(cert);
                        }
                    );
                })
                .UseStartup<Startup>()
                .Build()
                .RunAsService();
        }

        private static void SetupLocalhostService()
        {
            new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseKestrel(options => { options.AllowSynchronousIO = true; })
                .UseUrls(new string[] {Service.Config.ServiceConfiguration.ServiceUri})
                .UseStartup<Startup>()
                .Build()
                .Run();
        }
    }
}