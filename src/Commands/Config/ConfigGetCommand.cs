using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System;

namespace ShieldCLI.Commands.Config
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
                var type = ShieldCommands.ChooseConfigurationType(settings.Type);
                var configName = $"shield.{type}.{settings.Name}.json";

                if (settings.Name is null)
                {
                    configName = ShieldCommands.GetFilesConfig(settings.Path);
                    if (configName == "") return 0;

                }


                var fullPath = ShieldCommands.CreateFullPath(settings.Path, configName);


                if (type == "application")
                {
                    ApplicationConfigurationDto appConfig = ShieldCommands.GetApplicationConfiguration(fullPath, settings.Create);

                    ShieldCommands.PrintConfigFiles(configName, appConfig.ProjectPreset, appConfig.Protections);
                }
                else
                {
                    ProjectConfigurationDto projectConfig = ShieldCommands.GetProjectConfiguration(fullPath, settings.Create);
                    ShieldCommands.PrintConfigFiles(configName, projectConfig.ProjectPreset, projectConfig.Protections);

                }


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

