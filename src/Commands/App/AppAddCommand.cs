using ShieldCLI.Helpers;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.App
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
