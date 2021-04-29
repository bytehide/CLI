using System;
using Dotnetsafer.CLI.Helpers;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI.Commands.Auth
{
    internal class AuthClearCommand : Command, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public AuthClearCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override int Execute(CommandContext context)
        {
            try
            {
                ShieldCommands.AuthClearCredentials();
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
