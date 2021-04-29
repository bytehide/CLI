using System;
using System.Threading.Tasks;
using Dotnetsafer.CLI.Helpers;
using Dotnetsafer.CLI.Models.Project;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI.Commands.Project
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
                _ = ShieldCommands.AuthHasCredentials();

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
