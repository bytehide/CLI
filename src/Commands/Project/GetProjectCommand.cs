using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;

namespace ShieldCLI.Commands.Project
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
                await ShieldCommands.ProjectFindOrCreateByIdAsync(getProjectOptions.Name, getProjectOptions.Key)
                : await ShieldCommands.FindOrCreateProjectByNameAsync(getProjectOptions.Name);

            ShieldCommands.PrintProject(project.Name, project.Key);
        }
    }
}
