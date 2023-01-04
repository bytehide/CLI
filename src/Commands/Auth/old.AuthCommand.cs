using Bytehide.CLI.Models;
using Bytehide.CLI.Models.Auth;
using MatthiWare.CommandLine.Abstractions.Command;
using Spectre.Console;

namespace Bytehide.CLI.Commands.Auth
{
    public class OldAuthCommand : Command<GlobalOptions, AuthOptions>
    {
        public ShieldCommands ShieldCommands { get; set; }

        public OldAuthCommand(ShieldCommands shieldCommands)
        
         =>   ShieldCommands = shieldCommands;
        

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        
        =>    builder.Name("auth").Description("Log or register a user in Shield.");
        

        public override void OnExecute(GlobalOptions options, AuthOptions auth)
        {
            if (auth.Register)
                ShieldCommands.AuthRegister();


            else if (auth.Clear)
                ShieldCommands.AuthClearCredentials();


            else if (auth.Check && ShieldCommands.AuthHasCredentials()) {
                    AnsiConsole.MarkupLine("[lime]You are logged in correctly[/]");
                    AnsiConsole.MarkupLine("");
            }

            else
                ShieldCommands.AuthLogin(auth.Login);

        }
    }
}


