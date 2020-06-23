using System.IO;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace playing
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Dns.GetHostAddresses(Dns.GetHostName());
            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())                
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
