using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;

namespace ShieldCLI.Commands.Project
{
    public class DeleteProjectCommand : Command<GlobalOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:delete").Description("Delete project");
        }

    }

}

