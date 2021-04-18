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

            ShieldCommands.ConfigMakeFile(options.Type, options.Path, options.Preset, options.Name);

        }

    }
}