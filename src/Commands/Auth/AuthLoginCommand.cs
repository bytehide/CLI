using System;
using System.ComponentModel;
using Bytehide.CLI.Helpers;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Auth
{
    internal class AuthLoginCommand : Command<AuthLoginCommand.AuthLoginCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        internal class AuthLoginCommandSettings : Branches.ShieldSettings
        {
            [CommandArgument(0, "<API TOKEN>"), Description("Your Bytehide personal api token.")]
            public string ApiToken { get; set; }
        }

        public AuthLoginCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        public override int Execute(CommandContext context, AuthLoginCommandSettings settings)
        {
            try
            {
                return ShieldCommands.AuthLogin(settings.ApiToken) ? 0 : 1;
            }
            catch (Exception e)
            {
                ExceptionHelpers.ProcessException(e);
                return 1;
            }
        }
    }
}
