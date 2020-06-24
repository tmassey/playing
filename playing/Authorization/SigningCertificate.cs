using System.IO;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.FileProviders;

namespace playing.Authorization
{
    public static class SigningCertificate
    {
        public static X509Certificate2 Load()
        {
            var certificate = ReadCertificate();
            return new X509Certificate2(certificate, GetCertificatePassword());
        }

        private static byte[] ReadCertificate()
        {
            var assembly = typeof(Startup).GetTypeInfo().Assembly;
            var embeddedFileProvider = new EmbeddedFileProvider(assembly, "playing");
            var certificateFileInfo = embeddedFileProvider.GetFileInfo("Authorization/SigningCerts/" + GetCertificateFileName());
            byte[] certificatePayload;
            using (var certificateStream = certificateFileInfo.CreateReadStream())
            {
                using (var memoryStream = new MemoryStream())
                {
                    certificateStream.CopyTo(memoryStream);
                    certificatePayload = memoryStream.ToArray();
                }
            }

            return certificatePayload;
        }


        private static string GetCertificateFileName()
        {
            var environment = Service.Config.ServiceConfiguration.IdentityEnvironment;
            switch (environment.ToUpper())
            {
                case "SANDBOX":
                    return "Sandbox.pfx";
                case "TESTING":
                    return "Testing.pfx";
                case "STAGING":
                    return "Production.pfx";
                case "PRODUCTION":
                    return "Production.pfx";
                default:
                    return "Sandbox.pfx";
            }
        }
        private static string GetCertificatePassword()
        {
            var environment = Service.Config.ServiceConfiguration.IdentityEnvironment;

            switch (environment.ToUpper())
            {
                case "SANDBOX":
                    return "vYjsx7ZWN4!TnAOw";
                case "TESTING":
                    return "5oWZNXR0ClOEZ%E";
                case "STAGING":
                    return "25cW*UFtYB&PNvJbici";
                case "PRODUCTION":
                    return "25cW*UFtYB&PNvJbici";
                default:
                    return "vYjsx7ZWN4!TnAOw";
            }
        }
    }
}
