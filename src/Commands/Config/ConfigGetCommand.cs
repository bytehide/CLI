using System;
using System.Linq;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Models.Config;
using Shield.Client.Models.API;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Config
{
    internal class ConfigGetCommand : Command<ConfigGetCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public ConfigGetCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }
        public override int Execute(CommandContext context, ConfigGetCommandSettings settings)
        {
            ShieldCommands.AuthHasCredentials();
            try
            {
                ProtectionConfigurationDTO config = null;

                if (settings.Name is null)
                    config = ShieldCommands.GetFilesConfig(settings.Path);
                else
                    config = ShieldCommands.GetFileConfig(settings.Path, settings.Name);

                if (config is null) return 0;

                if (string.IsNullOrEmpty(config.Name) || string.IsNullOrWhiteSpace(config.Name))
                    config = config.Rename("Empty name");

                 ShieldCommands.PrintConfigFiles(config.Name, config.Preset, config.ConfigurationType.ToString(), config.Protections);

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

