using System.Threading;
using System.Threading.Tasks;
using Bytehide.CLI.Models;
using Bytehide.CLI.Models.Project;
using MatthiWare.CommandLine.Abstractions.Command;

namespace Bytehide.CLI.Commands.Project
{
    public class GetProjectCommand : Command<GlobalOptions, GetProjectOptions>
    {
        public ShieldCommands ShieldCommands { get; set; }

        public GetProjectCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:find").Description("Projects Management");
        }
        public override async Task OnExecuteAsync(GlobalOptions option, GetProjectOptions getProjectOptions, CancellationToken cancellationToken)
        {
            ShieldCommands.AuthHasCredentials();

            var project = getProjectOptions.Key != null ?
                await ShieldCommands.FindOrCreateProjectByIdAsync(getProjectOptions.Name, getProjectOptions.Key)
                : await ShieldCommands.FindOrCreateProjectByNameAsync(getProjectOptions.Name);

            ShieldCommands.PrintProject(project.Name, project.Key);
        }
    }
}
