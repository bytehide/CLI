using System;
using System.Threading.Tasks;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Models.App;
using Bytehide.CLI.Repos;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.App
{
    internal class AppAddCommand : AsyncCommand<AppAddCommandSettings>, ICommandLimiter<ShieldSettings>

    {
        public ShieldCommands ShieldCommands { get; }
        public ClientManager ClientManager { get; }

        public AppAddCommand(ShieldCommands shieldCommands, ClientManager clientManager)
        {
            ShieldCommands = shieldCommands;
            ClientManager = clientManager;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, AppAddCommandSettings settings)
        {
            try
            {
                _ = ShieldCommands.AuthHasCredentials();

                var projectKey = settings.Project;

                if (!settings.IsProjectKey)
                {
                    var project = await ShieldCommands.FindOrCreateProjectByNameAsync(settings.Project);
                    projectKey = project.Key;
                }

                await ShieldCommands.UploadApplicationAsync(settings.ApplicationPath, projectKey);

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
