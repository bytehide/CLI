using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Config
{
    public class GetConfigCommand : Command<GlobalOptions, GetConfigOptions>

    {

        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public GetConfigCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:find").Description("Get a config from a path");
        }

        public override void OnExecute(GlobalOptions option, GetConfigOptions options)
        {
            ShieldCommands.AuthHasCredentials();

            // falta hacer un check de que el archivo existe o no existe 

            ShieldCommands.ConfigGetFile(options.Type, options.Path, options.Name);



        }
    }
}
