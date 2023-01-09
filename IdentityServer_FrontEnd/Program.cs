using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace IdentityServer_FrontEnd
{
    //https://www.youtube.com/watch?v=KNt1V72ZRAY
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            host.Run();
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                    });
    }
}

