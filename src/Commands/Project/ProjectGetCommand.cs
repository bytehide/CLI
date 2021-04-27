using ShieldCLI.Helpers;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.Project
{
    internal class ProjectGetCommand : AsyncCommand<ProjectGetCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public ProjectGetCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }
        public async override Task<int> ExecuteAsync(CommandContext context, ProjectGetCommandSettings settings)
        {

            try
            {
                _ = ShieldCommands.AuthHasCredentials();
                var project = !settings.IsProjectKey ?
                         await ShieldCommands.FindOrCreateProjectByNameAsync(settings.Project) : await ShieldCommands.FindOrCreateProjectByIdAsync("default", settings.Project);

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

