using IdentityServer_Common.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace API
{
    public class Program
    {
        public static int Main(string[] args)
        {
            AdditionalClasses.AddAndConfiguredLogger();
            //AdditionalClasses.OpenBrowser("http://localhost:7249/Hello");

            try
            {
                var host = CreateHostBuilder(args).Build();

                Log.Information("������� ������ ����� API...");

                host.Run();

                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "���� API �������� ���� ������ �� �������");
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

