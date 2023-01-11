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
    }
}
