using FluentValidation;
using IdentityServer_Common.Constants;
using IdentityServer_Common.Extensions;
using IdentityServer_Common.Resources;
using IdentityServer_DAL.Data;
using IdentityServer_DAL.Entity;
using IdentityServer_FrontEnd_Common.Entity.ViewModel.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

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
            services.AddControllersWithViews(conf =>
            {
                // Якщо нема ? не буде рахувати помилкою
                conf.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });

            services.AddValidatorsFromAssemblyContaining<LoginViewModel>();
            services.AddValidation();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connetionString = Configuration.GetConnectionString(IdentityServerFrontEndConstants.StringConnectionMySQL);
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
                config.Cookie.Name = IdentityServerFrontEndConstants.CookieName;
                config.LoginPath = new PathString($"/{IdentityServerFrontEndConstants.ControllerNameAuth}/{IdentityServerFrontEndConstants.NamePageLogin}");
                config.LogoutPath = new PathString($"/{IdentityServerFrontEndConstants.ControllerNameAuth}/LogOut");
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

                options.UserInteraction.LoginUrl = new PathString($"/{IdentityServerFrontEndConstants.ControllerNameAuth}/{IdentityServerFrontEndConstants.NamePageLogin}");
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

            
            //app.UseStaticFiles();
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "Styles")),
                RequestPath = "/Styles"
            });
            app.UseRouting();
            
            app.UseIdentityServer();
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapGet("/Hello", async context =>
                {
                    await context.Response.WriteAsync("Hello Vitaliy it`s IdentityServer!");
                });
            });
        }
    }
}