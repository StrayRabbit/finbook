using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace User.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                //.UseKestrel(options =>
                //{
                //    // Set properties and call methods on options
                //    options.Limits.MaxConcurrentConnections = 100;
                //})
                .UseUrls("http://+:9001")
                .Build();
    }
}
