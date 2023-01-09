using IdentityServer_Common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;

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
            services.AddSwaggerGen(opt => 
            {
                opt.AddSecurityDefinition("Oauth", new OpenApiSecurityScheme
                {
                    Flows = new OpenApiOAuthFlows
                    {
                        ClientCredentials = new OpenApiOAuthFlow
                        {
                            TokenUrl = new Uri("https://localhost:5001/connect/token")
                        }
                    },
                    Type = SecuritySchemeType.OAuth2
                });

                opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference 
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Oauth"
                            }
                        },
                        new[] 
                        {
                            "profile", "openId" 
                        }
                    }
                });
            });

            /* Працює, всее добре просто зміню на авторизацію лог пас
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer( options =>
                {
                    options.Authority = "https://localhost:5001";
                    
                    ///* Для контролю отримання токена
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
                        // Додати поле з токеном у відповідь
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
                    //*

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                        NameClaimType = "TestNameCliaim",
                        RoleClaimType = "TestRole"
                    };
                    options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };
                });
            */

            ///////////////////////////////////код повязаний з авторизацією через логін пароль//////////////////////////
            services.AddAuthentication(conf =>
            {
                conf.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                conf.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, option =>
                {
                    option.Authority = "https://localhost:5001";
                    option.RequireHttpsMetadata = false;
                });
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////

            services.AddAuthorization(options =>
            {
                options.AddPolicy("ApiScope", policy =>
                {
                    policy.RequireClaim("scope", IdentityServerScopeConstants.ApiScope_Level1);
                });
            });

            /* Налаштувати роботу Cookie
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = new PathString("/Account/Login");
                });
            */
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
                endpoints.MapControllers();
            });
        }
    }
}