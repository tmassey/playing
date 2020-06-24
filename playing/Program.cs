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
                .UseKestrel(options=>options.Listen(IPAddress.Any,17209))
                .UseContentRoot(Directory.GetCurrentDirectory())                
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
