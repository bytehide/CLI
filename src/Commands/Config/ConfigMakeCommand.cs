using ShieldCLI.Commands;
using ShieldCLI.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Diagnostics.CodeAnalysis;

namespace ShieldCLI
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
                string[] protectionsID = { };
                if (preset == "custom")
                {
                    var projectKey = ShieldCommands.FindOrCreateProjectByName("default").Key;

                    protectionsID = ShieldCommands.ChooseCustomProtections(projectKey);
                }

                if (type == "application")
                {
                    _ = ShieldCommands.MakeApplicationConfiguration(settings.Path, preset, settings.Name, protectionsID);
                }
                else
                {
                    _ = ShieldCommands.MakeProjectConfiguration(settings.Path, preset, settings.Name, protectionsID);
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
