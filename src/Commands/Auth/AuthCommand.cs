using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Auth;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Auth
{
    public class AuthCommand : Command<GlobalOptions, AuthOptions>
    {

        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public AuthCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("auth").Description("Log or register a user in Shield.");
        }

        public override void OnExecute(GlobalOptions options, AuthOptions auth)
        {
            if (auth.Register)
                ShieldCommands.AuthRegister();


            if (auth.Clear)
                ShieldCommands.AuthClearCredentials();


            if (auth.Check)
                if (ShieldCommands.AuthHasCredentials())
                    AnsiConsole.MarkupLine("[lime]You are logged in correctly[/]");
            AnsiConsole.MarkupLine("");

            if (auth.Login != null)
                ShieldCommands.AuthDoLogin(auth.Login);

        }
    }
}


