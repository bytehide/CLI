using System.Threading.Tasks;
using Bytehide.CLI.Models.Protect;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Protect
{
    internal class ProtectCommand : AsyncCommand<ProtectCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public ProtectCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override async Task<int> ExecuteAsync(CommandContext context, ProtectCommandSettings settings)
        {
            _ = ShieldCommands.AuthHasCredentials();

            var projectKey = settings.Project;
            var pathApp = settings.ApplicationPath;
            var pathOutput = settings.OutputPath;

            if (!settings.IsProjectKey)
            {
                var project = await ShieldCommands.FindOrCreateProjectByIdAsync("default", projectKey);
                projectKey = project.Key;
            }

            var appUpload = await ShieldCommands.UploadApplicationAsync(pathApp, projectKey);

            var config = ShieldCommands.GetUniversalConfiguration(settings.ConfigurationPath);

            await ShieldCommands.ProtectApplicationAsync(projectKey, appUpload.ApplicationBlob, config, pathOutput);

            return 0;
        }

    }
}
