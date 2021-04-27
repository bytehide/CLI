using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Helpers;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

                if (type == "application")
                {
                    ApplicationConfigurationDto appConfig = ShieldCommands.GetApplicationConfiguration(settings.Path, settings.Name, settings.Create);

                    Console.WriteLine("archivo de application");
                }
                else
                {

                    ProjectConfigurationDto proConfig = ShieldCommands.GetProjectConfiguration(settings.Path, settings.Name, settings.Create);
                    Console.WriteLine("archivo de project");

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

