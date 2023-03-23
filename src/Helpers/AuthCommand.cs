using Bytehide.CLI.Commands;
using Bytehide.CLI.Models.Config;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Bytehide.CLI.Helpers
{
    internal class AuthenticableShieldCommand : Command<ConfigMakeCommandSettings>, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }

        public AuthenticableShieldCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }

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
