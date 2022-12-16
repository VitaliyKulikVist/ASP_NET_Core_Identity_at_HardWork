using ASP_NET_Core_Identity_at_HardWork.Data;
using ASP_NET_Core_Identity_at_HardWork.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ASP_NET_Core_Identity_at_HardWork
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
            
            services.AddControllersWithViews();

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                var connetionString = Configuration.GetConnectionString("DefaultConnection");
                options.UseMySql(connetionString);
            });
            
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            
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

                options.EmitStaticAudienceClaim = true;
            })
                .AddInMemoryIdentityResources(Config.IdentityResources)
                .AddInMemoryApiScopes(Config.ApiScopes)
                .AddInMemoryClients(Config.Clients)
                .AddAspNetIdentity<ApplicationUser>();

            
            builder.AddDeveloperSigningCredential();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    // базова адреса сервера ідентифікації
                    //options.Authority = "https://demo.identityserver.io";

                    // якщо використовуюьться ресурси API, ви можете вказати назву тут
                    //options.Audience = IdentityConstants.ApiScope_Level1;


                    options.Authority = Configuration["Authority"];
                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {

                            var accessToken = context.Request.Query["access_token"];

                            var path = context.HttpContext.Request.Path;
                            if (!string.IsNullOrEmpty(accessToken) && (path.StartsWithSegments("/chathub")))
                            {
                                context.Token = accessToken;
                            }

                            //тільки для дебагу просто щоб подивитись як працює
                            Log.Information("Message Received, AccessToken = {AccessToken}", accessToken);

                            return Task.CompletedTask;
                        },
                        OnTokenValidated = context =>
                        {
                            var token = context.SecurityToken as JwtSecurityToken;
                            if (token != null)
                            {
                                ClaimsIdentity identity = context.Principal.Identity as ClaimsIdentity;
                                if (identity != null)
                                {
                                    identity.AddClaim(new Claim("access_token", token.RawData));
                                }
                            }

                            //тільки для дебагу просто щоб подивитись як працює
                            Log.Information("Token Validated {rawData}", token.RawData);

                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            var textEror = context.Exception.Message;

                            //тільки для дебагу просто щоб подивитись як працює
                            Log.Error("Authentication Failed, becouse \t{failed}", textEror);

                            return Task.CompletedTask;
                        }
                    };
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "name",
                        RoleClaimType = "role"
                    };
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

            /* Перевірка дійсності токена
            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "SignalR.API");
                });
            });
            */

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }

            
            app.UseStaticFiles();

            app.UseRouting();
            
            app.UseIdentityServer();
            
            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}