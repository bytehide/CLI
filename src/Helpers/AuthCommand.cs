using Bytehide.CLI.Commands;
using Bytehide.CLI.Models.Config;
using Spectre.Console.Cli;
using System.Diagnostics;
using System.Reflection;

namespace Bytehide.CLI.Helpers
{
    internal class AuthenticableShieldCommand : Command<ConfigMakeCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public AuthenticableShieldCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

        /// <summary>
        /// Checks for authentication before exec the original command
        /// </summary>
        /// <param name="context"></param>
        /// <param name="settings"></param>
        /// <returns></returns>
        public override int Execute(CommandContext context, ConfigMakeCommandSettings settings)
        {
            var method = new StackTrace().GetFrame(1).GetMethod();

            var auth = method.GetCustomAttribute<AuthRequiredAttribute>();

            if (auth != null && auth.Service == AuthService.Shield)
                return ShieldCommands.AuthHasCredentials() ? 0 : 1;

            return 0;
        }
       
    }
}
