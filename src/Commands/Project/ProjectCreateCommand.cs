using ShieldCLI.Helpers;
using ShieldCLI.Models.Project;
using Spectre.Console.Cli;
using System;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.Project
{
    internal class ProjectCreateCommand : AsyncCommand<ProjectCreateCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }


        public ProjectCreateCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;

        }

        public override async Task<int> ExecuteAsync(CommandContext context, ProjectCreateCommandSettings settings)
        {
            try
            {
                ShieldCommands.AuthHasCredentials();

                var project = await ShieldCommands.FindOrCreateProjectByNameAsync(settings.Project);

                ShieldCommands.PrintProject(project.Name, project.Key);
                return 0;

            }
            catch (Exception e)
            {
                ExceptionHelpers.ProcessException(e);
                return 1;
            }
        }
    }
}
