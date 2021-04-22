using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Protect;
using ShieldCLI.Repos;
using Spectre.Console;
using Shield.Client.Extensions;
using System.Threading;

namespace ShieldCLI.Commands.Protect
{
    public class ProtectCommand : Command<GlobalOptions, ProtectOptions>
    {

        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public ProtectCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect").Description("Protect an application or project.");



        }

        public override void OnExecute(GlobalOptions option, ProtectOptions options)
        {
        }
    }

}