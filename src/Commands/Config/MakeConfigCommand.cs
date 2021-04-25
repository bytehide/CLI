using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Repos;

namespace ShieldCLI.Commands.Config
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

            var type = ShieldCommands.ChooseType(options.Type);
            var preset = ShieldCommands.ChoosePreset(options.Preset);

            var protectionsId = ShieldCommands.ChooseCustomProtections("ad955664-b28b-46a4-86d1-489eb7306f6e");

            if (type == "application")
            {
                var config = ShieldCommands.ConfigApplicationMakeFile(options.Path, preset, options.Name, protectionsId);
            }
            else
            {
                var config = ShieldCommands.ConfigProjectMakeFile(options.Path, preset, options.Name, protectionsId);
            }

        }


    }
}