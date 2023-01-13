using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;
using IdentityServer_DAL.Data;
using Microsoft.EntityFrameworkCore;
using IdentityServer_DAL.Entity;
using IdentityServer_Common.Resources;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using IdentityServer_DAL.Entity.Auth;
using IdentityServer_Common.Extensions;

namespace IdentityServer_FrontEnd
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
            services.AddControllersWithViews(conf =>
            {
                // Якщо нема ? не буде рахувати помилкою
                conf.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; 
            });

            services.AddValidatorsFromAssemblyContaining<LoginViewModel>();
            services.AddValidation();

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
                config.LoginPath = new PathString("/Auth/Login");
                config.LogoutPath = new PathString("/Auth/Logout");
            });


            var builder = services.AddIdentityServer(options =>
            {
                options.Events.RaiseErrorEvents = true;
                options.Events.RaiseInformationEvents = true;
                options.Events.RaiseFailureEvents = true;
                options.Events.RaiseSuccessEvents = true;

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

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "Styles")),
                RequestPath = "/styles"
            });
            app.UseRouting();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapGet("/Hello", async context =>
                {
                    await context.Response.WriteAsync("Hello Vitaliy it`s FrontEnd pages!");
                });
            });
        }
    }
}