using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class GetConfigCommand : Command<GlobalOptions, GetConfigOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:find").Description("Get a config from a path");
        }
    }
}
