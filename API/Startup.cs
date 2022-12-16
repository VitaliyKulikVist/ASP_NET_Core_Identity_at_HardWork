using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API
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
            services.AddControllers();

            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    //options.Authority = "https://localhost:5001";

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
                                ClaimsIdentity? identity = context.Principal!.Identity as ClaimsIdentity;
                                if (identity != null)
                                {
                                    identity.AddClaim(new Claim("access_token", token.RawData));
                                }
                            }

                            //тільки для дебагу просто щоб подивитись як працює
                            Log.Information("Token Validated {rawData}", token!.RawData);

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
                        ValidateAudience = false
                        //NameClaimType = "testName",
                        //RoleClaimType = "testRole"
                    };
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });

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
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseRouting();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers()
                .RequireAuthorization("Доступ Першого рівня");
            });
        }
    }
}