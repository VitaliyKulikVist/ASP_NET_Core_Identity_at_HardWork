using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace ASP_NET_Core_Identity_at_HardWork
{
    public class Program
    {

        public static int Main(string[] args)
        {
            AddAndConfiguredLogger();

            try
            {
                var seed = args.Contains("/seed");
                if (seed)
                {
                    args = args.Except(new[] { "/seed" }).ToArray();
                }

                var host = CreateHostBuilder(args).Build();

                if (seed)
                {
                    SeedingDataAtBD(host);

                    Console.ReadKey();

                    return 0;
                }

                Log.Information("Початок роботи Хоста...");
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

        private static void SeedingDataAtBD(IHost host)
        {
            var timeStart = DateTime.UtcNow;
            Log.Information("Заповнення бази данних...\t{timeStart}", timeStart);
            var config = host.Services.GetRequiredService<IConfiguration>();
            var connectionString = config.GetConnectionString("DefaultConnection");
            SeedData.EnsureSeedData(connectionString);
            var timeFinish = DateTime.UtcNow;
            Log.Information("База данних заповнена.\t{timeFinish}", timeFinish);
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