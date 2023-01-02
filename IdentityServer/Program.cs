using IdentityServer.MenegmentData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;

namespace IdentityServer
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

                var delete = args.Contains("/delete");
                if (delete)
                {
                    args = args.Except(new[] { "/delete" }).ToArray();
                }

                var host = CreateHostBuilder(args).Build();

                if (seed)
                {
                    SeedingDataAtBD("server=localhost;user=vitaliy;password=12345678;database=TestConnectionName;");

                    return 0;
                }

                else
                if(delete)
                {
                    DeletingDataAtBD("server=localhost;user=vitaliy;password=12345678;database=TestConnectionName;");

                    return 0;
                }

                Log.Information("Початок роботи Хоста IdentityServer...");
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
            AnsiConsoleTheme theme = AnsiConsoleTheme.Literate;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: theme)
                .CreateLogger();
        }

        private static void SeedingDataAtBD(string connectionString)
        {
            var timeStart = DateTime.UtcNow;
            Log.Information("Заповнення бази данних...\t{timeStart}", timeStart);
            SeedData.EnsureSeedData(connectionString);
            var timeFinish = DateTime.UtcNow;
            Log.Information("База данних заповнена.\t{timeFinish}", timeFinish);
        }

        private static void DeletingDataAtBD(string connectionString)
        {
            var timeStart = DateTime.UtcNow;
            Log.Information("Видалення данних з бд...\t{timeStart}", timeStart);
            DeleteData.DeleteAllUsers(connectionString);
            var timeFinish = DateTime.UtcNow;
            Log.Information("Данні видалено.\t{timeFinish}", timeFinish);
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