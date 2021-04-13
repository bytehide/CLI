using System.Diagnostics;
using System.Runtime.InteropServices;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Auth;

namespace ShieldCLI.Commands.Auth
{
    public class AuthCommand : Command<GlobalOptions, AuthOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("auth").Description("Log or register a user in Shield.");
        }
        // En la ejecucion ( sera asíncrona) se trae en el param register los que tengamos en AuthOptions y los pasa a la función de ShieldClient

        public override void OnExecute(GlobalOptions options, AuthOptions register)
        {
            if (register.Register)
            {
                OpenBrowser("https://my.dotnetsafer.com/register");
               
            }

        }
        // esto es para abrir web en navegador
        public void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                Process.Start("xdg-open", url);  // Works ok on linux
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                Process.Start("open", url); // Not tested
            }
     
        }


    }
}
