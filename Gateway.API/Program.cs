using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace Gateway.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((webhost, builder) =>
                {
                    builder
                        .SetBasePath(webhost.HostingEnvironment.ContentRootPath)
                        .AddJsonFile("Ocelot.json");
                })
                .UseUrls("http://+:90")
                .UseStartup<Startup>()
                .Build();
    }
}
