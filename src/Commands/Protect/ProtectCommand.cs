using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class ProtectCommand : Command<GlobalOptions, ProtectOptions> {


        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect").Description("Protect an application or project  ");


      
        }
    }
}
