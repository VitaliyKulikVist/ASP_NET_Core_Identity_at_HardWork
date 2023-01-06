using IdentityServer.Resources;
using IdentityServer_DAL.Data;
using IdentityServer_DAL.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace IdentityServer
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllersWithViews();//Без цих сервісів не працює app.UseAuthorization();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connetionString = Configuration.GetConnectionString("DefaultConnectionMySQL");
                options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = 6;//Мінімальна довжина пароля
                config.Password.RequireDigit = false;//Не обов'язково використовувати ЦИФРИ
                config.Password.RequireNonAlphanumeric = false;//Не обов'язково використовувати СИМВОЛИ
                config.Password.RequireUppercase = false;//Не обов'язково використовувати ВЕЛИКІ букви

            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = "Identity.Test_Cookie_Name";
                config.LoginPath = new PathString("/Account/Login");
                config.LogoutPath = new PathString("/Account/Logout");
            });

            
            var builder = services.AddIdentityServer(options =>
            {
                //Чи викликати події помилки.
                options.Events.RaiseErrorEvents = true;
                //Чи потрібно викликати інформаційні події.
                options.Events.RaiseInformationEvents = true;
                //Чи потрібно викликати події збою.
                options.Events.RaiseFailureEvents = true;
                //Чи потрібно викликати події успіху.
                options.Events.RaiseSuccessEvents = true;

                //Видає претензію aud із форматом видавця/ресурсів. Це потрібно для деяких старих систем перевірки маркерів доступу. За замовчуванням значення false.
                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            
            builder.AddDeveloperSigningCredential();
        }

        public void Configure(IApplicationBuilder app)
        {
            
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            
            app.UseStaticFiles();
            app.UseRouting();
            
            app.UseIdentityServer();
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapGet("/Hello", async context =>
                {
                    await context.Response.WriteAsync("Hello Vitaliy!");
                });
            });
        }
    }
}