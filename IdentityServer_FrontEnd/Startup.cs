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
using IdentityServer_Common.Extensions;
using IdentityServer_DAL.Entity.ViewModel.Auth;
using IdentityServer_Common.Constants;
using IdentityServer_Common.Common;

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
            services.AddRazorPages()
                .AddRazorRuntimeCompilation();//Потім не забути видалити

            services.AddControllersWithViews(conf =>
            {
                // Якщо нема ? не буде рахувати помилкою
                conf.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true; 
            });

            services.AddValidatorsFromAssemblyContaining<LoginViewModel>();
            services.AddValidation();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connetionString = Configuration.GetConnectionString(FrontEndConstants.StringConnectionMySQL);
                options.UseMySql(connetionString, ServerVersion.AutoDetect(connetionString));
            });

            services.AddIdentity<ApplicationUser, IdentityRole>(config =>
            {
                config.Password.RequiredLength = FluentValidationConstants.MinimumLengthPassword;//Мінімальна довжина пароля
                config.Password.RequireDigit = false;//Не обов'язково використовувати ЦИФРИ
                config.Password.RequireNonAlphanumeric = false;//Не обов'язково використовувати СИМВОЛИ
                config.Password.RequireUppercase = false;//Не обов'язково використовувати ВЕЛИКІ букви
                config.Password.RequireLowercase = false;//чи паролі повинні містити символ ASCII нижнього регістру
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.ConfigureApplicationCookie(config =>
            {
                config.Cookie.Name = FrontEndConstants.CookieName;
                config.LoginPath = new PathString($"/{FrontEndConstants.ControllerNameAuth}/{FrontEndConstants.NamePageLogin}");
                config.LogoutPath = new PathString($"/{FrontEndConstants.ControllerNameAuth}/LogOut");
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

            app.UseStaticFiles();
            //app.UseStaticFiles( new StaticFileOptions
            //{
            //    FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "Styles")),
            //    RequestPath = "/Styles"
            //});


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