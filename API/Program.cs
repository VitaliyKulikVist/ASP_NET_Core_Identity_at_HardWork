using Microsoft.Extensions.Hosting;
using System;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog.Events;
using Microsoft.AspNetCore.Hosting;

namespace API
{
    public class Program
    {
        public static int Main(string[] args)
        {
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
                Log.Fatal(ex, "Host terminated unexpectedly.");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }
        private static void AddAndConfiguredLogger()
        {
            AnsiConsoleTheme theme = AnsiConsoleTheme.Code;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Fatal)
                .MinimumLevel.Override("System", LogEventLevel.Fatal)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Fatal)
                .Enrich.FromLogContext()

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: theme)
                .CreateLogger();
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

