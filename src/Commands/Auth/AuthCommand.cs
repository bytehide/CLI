using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Auth;
using Spectre.Console;

namespace ShieldCLI.Commands.Auth
{
    public class AuthCommand : Command<GlobalOptions, AuthOptions>
    {
        public ShieldCommands ShieldCommands { get; set; }

        public AuthCommand(ShieldCommands shieldCommands)
        
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


