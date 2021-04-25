using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Project
{
    public class GetProjectCommand : Command<GlobalOptions, GetProjectOptions>
    {
        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public GetProjectCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:find").Description("Projects Management");
        }
        public override async Task OnExecuteAsync(GlobalOptions option, GetProjectOptions getProjectOptions, CancellationToken cancellationToken)
        {
            if (!ClientManager.HasValidClient())
            {
                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };


            var project = getProjectOptions.Key != null ?
                await ShieldCommands.ProjectFindOrCreateByIdAsync(getProjectOptions.Name, getProjectOptions.Key)
                : await ShieldCommands.ProjectFindOrCreateByNameAsync(getProjectOptions.Name);

            ShieldCommands.ProjectTable(project.Name, project.Key);
        }
    }
}
