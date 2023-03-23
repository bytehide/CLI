using System;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Models.Config;
using Shield.Client.Models.API;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Config
{
    internal class ConfigMakeCommand : AuthenticableShieldCommand
    {
        public ConfigMakeCommand(ShieldCommands shieldCommands) : base(shieldCommands){}

        [AuthRequired]
        public override int Execute(CommandContext context, ConfigMakeCommandSettings settings)
        {
            try
            {
                base.Execute(context, settings);

                var type = ShieldCommands.ChooseConfigurationType(settings.Type);

                var preset = ShieldCommands.ChooseProtectionPreset(settings.Preset);

                string[] protectionsId = Array.Empty<string>();

                if (preset == "custom")
                {
                    var projectKey = ShieldCommands.AsMute().FindOrCreateProjectByName("default").Key;

                    protectionsId = ShieldCommands.ChooseCustomProtections(projectKey);
                }

                _ = ShieldCommands.MakeUniversalConfiguration(settings.Path, settings.Name, preset, type ?? ConfigurationType.Application, protectionsId);

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
