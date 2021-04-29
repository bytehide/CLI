using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Dotnetsafer.CLI.Helpers
{
    public static class UsefulHelpers
    {
        public static void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url); // Works ok on linux
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Process.Start("open", url); // Not tested
        }
    }
}
