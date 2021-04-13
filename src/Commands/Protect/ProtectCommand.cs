using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Protect;

namespace ShieldCLI.Commands.Protect
{
    public class ProtectCommand : Command<GlobalOptions, ProtectOptions> {


        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect").Description("Protect an application or project  ");


      
        }
    }
}
