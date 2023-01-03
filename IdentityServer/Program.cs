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

            bool haveArgs = args.Length > 0;

            try
            {
                if(haveArgs)
                {
                    return ControllArgs(args);
                }

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
            Log.Information("Видалення данних з бази данних...\t{timeStart}", timeStart);
            DeleteData.DeleteAllUsers(connectionString);
            var timeFinish = DateTime.UtcNow;
            Log.Information("Данні видалено.\t{timeFinish}", timeFinish);
        }

        private static int ControllArgs(string[] args)
        {
            var seed = args.Contains(ConstantIdentityServer.CommandSeeding);
            if (seed)
            {
                args = args.Except(new[] { ConstantIdentityServer.CommandSeeding }).ToArray();
            }

            var delete = args.Contains(ConstantIdentityServer.CommandDeleting);
            if (delete)
            {
                args = args.Except(new[] { ConstantIdentityServer.CommandDeleting }).ToArray();
            }

            if (seed)
            {
                SeedingDataAtBD(ConstantIdentityServer.ConnectionMySQL);

                return 0;
            }

            else
            if (delete)
            {
                DeletingDataAtBD(ConstantIdentityServer.ConnectionMySQL);

                return 0;
            }

            return 0;
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