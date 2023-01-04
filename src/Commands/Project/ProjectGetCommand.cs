using System;
using System.Threading.Tasks;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Models.Project;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Project
{
    internal class ProjectGetCommand : AsyncCommand<ProjectGetCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public ProjectGetCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, ProjectGetCommandSettings settings)
        {
            try
            {
                _ = ShieldCommands.AuthHasCredentials();
                var project = !settings.IsProjectKey ?
                         await ShieldCommands.FindOrCreateProjectByNameAsync(settings.Project) :
                         await ShieldCommands.FindOrCreateProjectByIdAsync("default", settings.Project);

                ShieldCommands.PrintProject(project.Name, project.Key);

                return 0;
            }
            catch (Exception ex)
            {
                ExceptionHelpers.ProcessException(ex);

                return 1;
            }
        }
    }
}

