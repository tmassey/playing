using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;

namespace Phoenix.Api.Core.Authorization
{
    public static class HttpsCertificate
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
                var certificateFileInfo = new FileInfo(Directory.GetCurrentDirectory() + "/Authorization/HttpsCerts/" + GetCertificateFileName());
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
            return "tmassey.mooo.com.pfx";
        }
        private static string GetCertificatePassword()
        {
            return "Poilkjmnb!1@";
            //return "a1e3i5o7u9";
        }
    }
}