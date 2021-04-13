using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.App;

namespace ShieldCLI.Commands.App
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("application:add").Description("Add an application to a project");
        }

    }
}