using IdentityModel;
using IdentityServer_DAL.Data;
using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;

namespace IdentityServer_DAL.MenegmentData
{
    public class SeedData
    {
        public static void EnsureSeedDataAsync(string connectionString = Constants.ConnectionMySQL)
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

                    foreach (var user in DefaultUsersData.UsersDictionary)
                    {
                        var unit = userMgr.FindByNameAsync(user.Key).Result;
                        if (unit == null)
                        {
                            unit = new ApplicationUser
                            {
                                UserName = user.Key,
                                Email = $"{user.Key}Smith@email.com",
                                Description = $"Опис користувача {user.Key}",
                                EmailConfirmed = true,
                            };
                            var result = userMgr.CreateAsync(unit, user.Value).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            result = userMgr.AddClaimsAsync(unit, new Claim[]{
                            new Claim(JwtClaimTypes.Name, $"{user.Key} Smith"),
                            new Claim(JwtClaimTypes.GivenName, user.Key),
                            new Claim(JwtClaimTypes.FamilyName, "Smith"),
                            new Claim(JwtClaimTypes.WebSite, $"http://{user.Key}.com"),
                        }).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            Console.WriteLine($"{user.Key} created");
                        }
                        else
                        {
                            Console.WriteLine($"{user.Key} already exists");
                        }
                    }
                }
            }
        }
    }
}
