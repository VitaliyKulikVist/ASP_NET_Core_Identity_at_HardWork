using IdentityServer_Common.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using System.IO;

namespace Client
{
    internal class Startup
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
                conf.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }


            app.UseStaticFiles();
            //app.UseStaticFiles(new StaticFileOptions
            //{
            //    //FileProvider = new PhysicalFileProvider(Path.Combine(Environment.ContentRootPath, "Styles")),
            //    FileProvider = new PhysicalFileProvider(Path.Combine($"{VisualStudioProvider.TryGetSolutionDirectory()}/IdentityServer_FrontEnd_Common", "Styles")),
            //    RequestPath = "/Styles"
            //});

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();

                endpoints.MapGet("/Hello", async context =>
                {
                    await context.Response.WriteAsync("Hello Vitaliy it`s Client pages!");
                });
            });
        }
    }
}