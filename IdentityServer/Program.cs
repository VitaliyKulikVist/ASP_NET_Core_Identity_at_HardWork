using IdentityServer_Common.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace IdentityServer
{
    public class Program
    {
        public static int Main(string[] args)
        {
            AdditionalClasses.AddAndConfiguredLogger();
            //AdditionalClasses.OpenBrowser("https://localhost:5001/Hello");
            //AdditionalClasses.OpenBrowser("https://localhost:5001/Auth/Login");

            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Початок роботи Хоста IdentityServer...");

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Помилка роботи хоста");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
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