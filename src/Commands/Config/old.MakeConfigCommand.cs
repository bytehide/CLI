using Dotnetsafer.CLI.Models;
using Dotnetsafer.CLI.Models.Config;
using Dotnetsafer.CLI.Repos;
using MatthiWare.CommandLine.Abstractions.Command;

namespace Dotnetsafer.CLI.Commands.Config
{
    public class MakeConfigCommand : Command<GlobalOptions, MakeConfigOptions>
    {
        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public MakeConfigCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:make").Description("Create a config settings file");
        }

        public override void OnExecute(GlobalOptions option, MakeConfigOptions options)
        {

            ShieldCommands.AuthHasCredentials();

            var type = ShieldCommands.ChooseConfigurationType(options.Type);
            var preset = ShieldCommands.ChooseProtectionPreset(options.Preset);

            var protectionsId = ShieldCommands.ChooseCustomProtections("ad955664-b28b-46a4-86d1-489eb7306f6e");

            if (type == "application")
            {
                var config = ShieldCommands.MakeApplicationConfiguration(options.Path, preset, options.Name, protectionsId);
            }
            else
            {
                var config = ShieldCommands.MakeProjectConfiguration(options.Path, preset, options.Name, protectionsId);
            }

        }


    }
}