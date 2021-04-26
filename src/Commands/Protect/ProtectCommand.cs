using System.IO;
using System.Threading.Tasks;
using ShieldCLI.Branches;
using ShieldCLI.Models.Protect;
using Spectre.Console.Cli;

namespace ShieldCLI.Commands.Protect
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
            var projectKey = settings.Project;
            var pathApp = settings.ApplicationPath;
            var configName = Path.GetFileName(settings.ConfigurationPath);
            var configDirectory = Path.GetDirectoryName(settings.ConfigurationPath);
            var pathOutput = settings.OutputPath;

            if (!settings.IsProjectKey)
            {
                var project = await ShieldCommands.FindOrCreateProjectByIdAsync("default", projectKey);
                projectKey = project.Key;
            }

            if (Path.GetDirectoryName(settings.ConfigurationPath) == "")
            {
                configName = $"Shield.Application.{settings.ConfigurationPath}.json";
                configDirectory = Path.GetDirectoryName(pathApp);
            }

            var appUpload = await ShieldCommands.UploadApplicationAsync(pathApp, projectKey);

            var config = ShieldCommands.GetApplicationConfiguration(configDirectory, configName, true);

            await ShieldCommands.ProtectApplicationAsync(projectKey, appUpload.ApplicationBlob, config, pathOutput);

            return 0;
        }

    }
}
