using System;
using System.Threading.Tasks;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Models.Project;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Project
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
