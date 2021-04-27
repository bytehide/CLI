using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;


namespace ShieldCLI.Commands.Project
{
    public class CreateProjectCommand : Command<GlobalOptions, CreateProjectOptions>
    {
        public ShieldCommands ShieldCommands { get; set; }
        public CreateProjectCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:make").Description("Create project");
        }


        public override async Task OnExecuteAsync(GlobalOptions options, CreateProjectOptions createOptions, CancellationToken cancellationToken)
        {
            ShieldCommands.AuthHasCredentials();

            var project = await ShieldCommands.FindOrCreateProjectByNameAsync(createOptions.Name);

            ShieldCommands.PrintProject(project.Name, project.Key);
        }

    }
}
