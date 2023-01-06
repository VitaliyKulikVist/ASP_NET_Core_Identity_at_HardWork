using IdentityServer_DAL.Configurations;
using IdentityServer_DAL.Data;
using IdentityServer_DAL_MySQL.MenegmentData;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace IdentityServer_DAL_MySQL
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

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

            Console.ForegroundColor= ConsoleColor.Green;
            Console.WriteLine($"Виберiть варiант роботи:");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"1: Заповнення бази данних данними");
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"2: Видалення з бази, данних");
            Console.ResetColor();

            int input = Convert.ToInt32( Console.ReadLine() );

            switch (input)
            {
                case 1:
                SeedData.EnsureSeedData(Constants.ConnectionMySQL);
                break; 
                case 2:
                DeleteData.DeleteAllUsers(Constants.ConnectionMySQL);
                break;
                default:
                break;
            }

            //host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}

