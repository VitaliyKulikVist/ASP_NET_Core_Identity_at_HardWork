using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.Extensions.FileProviders;

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
            services.AddControllersWithViews();

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

                endpoints.MapGet("/HelloFront", async context =>
                {
                    await context.Response.WriteAsync("Hello Vitaliy it`s FrontEnd pages!");
                });
            });
        }
    }
}