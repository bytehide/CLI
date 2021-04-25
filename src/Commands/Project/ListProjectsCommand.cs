using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;

namespace ShieldCLI.Commands.Project
{
    public class ListProjectsCommand : Command<GlobalOptions>
    {
        private ShieldCommands ShieldCommands { get; }

        public ListProjectsCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:list").Description("List all projects");
        }

        public override void OnExecute(GlobalOptions options)
        {
            ShieldCommands.AuthHasCredentials();

            //TODO: @jespanag  Create method to get projects on Client
        }
    }
}
