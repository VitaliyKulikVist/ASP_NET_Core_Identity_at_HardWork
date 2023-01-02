using IdentityServer.Data;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace IdentityServer.MenegmentData
{
    public class DeleteData
    {
        public static void DeleteAllUsers(string connectionString)
        {
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddDbContext<ApplicationDbContext>(options =>
               options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            using (var serviceProvider = services.BuildServiceProvider())
            {
                using (var scope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    var context = scope.ServiceProvider.GetService<ApplicationDbContext>();

                    if (context == null)
                    {
                        throw new InvalidOperationException(nameof(context));
                    }

                    context.Database.Migrate();

                    var userMgr = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                    foreach (var user in UsersData.UsersDictionary)
                    {
                        var unit = userMgr.FindByNameAsync(user.Key).Result;
                        if (unit != null)
                        {
                            var result = userMgr.DeleteAsync(unit).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }
                        }
                    }
                }
            }
        }
    }
}
