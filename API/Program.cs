using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using IdentityServer_Common.Common;

namespace API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            AdditionalClasses.AddAndConfiguredLogger();

            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("Початок роботи Хоста API...");

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Хост API завершив свою роботу по помилці");
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

