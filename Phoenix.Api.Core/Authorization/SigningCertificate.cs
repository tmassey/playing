using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Phoenix.Api.Core.Authorization
{
    public static class SigningCertificate
    {
        public static X509Certificate2 Load()
        {
            
            try
            {
                var rawCertificate = ReadCertificate();
               
                return new X509Certificate2(rawCertificate, GetCertificatePassword());
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                
            }
            
            return null;
        }

        private static string ReadCertificate()
        {
            try
            {
                var certificateFileInfo = new FileInfo(Directory.GetCurrentDirectory() + "/Authorization/SigningCerts/" + GetCertificateFileName());
                return certificateFileInfo.FullName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
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
