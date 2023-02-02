using IdentityServer_DAL.Data;
using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace IdentityServer_DAL_MySQL.MenegmentData
{
    public class DeleteData
    {
        public async void DeleteAllUsersAsync(string connectionString = Constants.ConnectionMySQL)
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
                        if (unit != null)
                        {
                            var result = userMgr.DeleteAsync(unit).Result;
                            if (!result.Succeeded)
                            {
                                throw new Exception(result.Errors.First().Description);
                            }

                            else
                            {
                                Console.WriteLine("deleted");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Не знайшло що видаляти");
                        }
                    }

                    if (userMgr.Users.Count() > 0)
                    {
                        Console.WriteLine("Є ще користувачі які потрібно видалити які не входять в список");

                        while (userMgr.Users.Count() > 0)
                        {
                            var identityResult = await userMgr.DeleteAsync(userMgr.Users.First());
  
                            if (identityResult != null && !identityResult.Succeeded)
                            {
                                throw new Exception(identityResult.Errors.First().Description);
                            }
                        }
                    }

                    
                }
            }
        }
    }
}
