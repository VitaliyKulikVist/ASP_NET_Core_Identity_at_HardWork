using IdentityServer_Common.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Client
{
    public class Program
    {
        public static void Main(string[] args)
        {
            AdditionalClasses.AddAndConfiguredLogger();

            var host = CreateHostBuilder(args).Build();
            host.Run();
        }


        private static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
