using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;
using ShieldCLI.Repos;


namespace ShieldCLI.Commands.Project
{
    public class CreateProjectCommand : Command<GlobalOptions, CreateProjectOptions>
    {
        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }
        public CreateProjectCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:make").Description("Create project");
        }


        public override async Task OnExecuteAsync(GlobalOptions options, CreateProjectOptions createOptions, CancellationToken cancellationToken)
        {
            ShieldCommands.AuthHasCredentials();


            var project = await ShieldCommands.ProjectFindOrCreateByNameAsync(createOptions.Name);

            ShieldCommands.ProjectTable(project.Name, project.Key);
        }

    }
}
