using IdentityServer_Common.Common;
using IdentityServer_DAL.Configurations;
using IdentityServer_DAL.Data;
using IdentityServer_DAL_MySQL.MenegmentData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System;

namespace IdentityServer_DAL_MySQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            AdditionalClasses.AddAndConfiguredLogger();

            #region Create DB
            using (var scope = host.Services.CreateScope())
            {
                var servicespProvider = scope.ServiceProvider;

                try
                {
                    var context = servicespProvider.GetRequiredService<ApplicationDbContext>();
                    DbInitializer.Initialize(context);
                }
                catch (Exception ex)
                {
                    var logger = servicespProvider.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "An error occurred while app initialization");
                }
            }
            #endregion

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

