using System;
using Dotnetsafer.CLI.Helpers;
using Dotnetsafer.CLI.Models.Config;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI.Commands.Config
{
    internal class ConfigMakeCommand : Command<ConfigMakeCommandSettings>, ICommandLimiter<ShieldSettings>
    {

        public ShieldCommands ShieldCommands { get; }


        public ConfigMakeCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;

        }
        public override int Execute(CommandContext context, ConfigMakeCommandSettings settings)
        {
            try
            {
                _ = ShieldCommands.AuthHasCredentials();

                var type = ShieldCommands.ChooseConfigurationType(settings.Type);
                var preset = ShieldCommands.ChooseProtectionPreset(settings.Preset);
                string[] protectionsId = { };
                if (preset == "custom")
                {
                    var projectKey = ShieldCommands.FindOrCreateProjectByName("default").Key;

                    protectionsId = ShieldCommands.ChooseCustomProtections(projectKey);
                }

                if (type == "application")
                {
                    _ = ShieldCommands.MakeApplicationConfiguration(settings.Path, preset, settings.Name, protectionsId);
                }
                else
                {
                    _ = ShieldCommands.MakeProjectConfiguration(settings.Path, preset, settings.Name, protectionsId);
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
