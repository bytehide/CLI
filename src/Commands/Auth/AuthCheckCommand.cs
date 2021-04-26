using System;
using ShieldCLI.Helpers;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ShieldCLI.Commands.Auth
{
    internal class AuthCheckCommand : Command, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public AuthCheckCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override int Execute(CommandContext context)
        {
            try
            {
                if (!ShieldCommands.AuthHasCredentials()) return 0;

                AnsiConsole.MarkupLine("[lime]You are logged in correctly[/]");
                AnsiConsole.MarkupLine("");
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
