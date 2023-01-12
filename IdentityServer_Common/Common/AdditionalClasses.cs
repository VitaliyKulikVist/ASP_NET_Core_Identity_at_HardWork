using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using Serilog;
using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace IdentityServer_Common.Common
{
    public static class AdditionalClasses
    {
        public static void OpenBrowser (string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) 
                { 
                    UseShellExecute = true 
                });
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url);
            }
            else
            {
                #if DEBUG
                        Console.WriteLine("Can`t open Browser pages\t {page}", url);
                #endif
            }
        }

        public static void AddAndConfiguredLogger(
            LogEventLevel microsoft = LogEventLevel.Warning,
            LogEventLevel microsoftHostingLifetime = LogEventLevel.Information,
            LogEventLevel system = LogEventLevel.Warning,
            LogEventLevel microsoftAspNetCoreAuthentication = LogEventLevel.Information)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", microsoft)
                .MinimumLevel.Override("Microsoft.Hosting.Lifetime", microsoftHostingLifetime)
                .MinimumLevel.Override("System", system)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", microsoftAspNetCoreAuthentication)
                .Enrich.FromLogContext()

                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();
        }
    }
}
