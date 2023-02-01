using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
//using IdentityServer_Common.Common;

namespace API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            //AdditionalClasses.AddAndConfiguredLogger();
            AddAndConfiguredLogger();

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

        public static void AddAndConfiguredLogger(
            LogEventLevel microsoft = LogEventLevel.Warning,
            LogEventLevel microsoftHostingLifetime = LogEventLevel.Information,
            LogEventLevel system = LogEventLevel.Warning,
            LogEventLevel microsoftAspNetCoreAuthentication = LogEventLevel.Information)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", microsoft)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", microsoftHostingLifetime)
                .MinimumLevel.Override("System", system)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", microsoftAspNetCoreAuthentication)
                .Enrich.FromLogContext()

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }
    }
}

