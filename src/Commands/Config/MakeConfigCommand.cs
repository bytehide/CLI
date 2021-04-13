using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.Config
{
    public class MakeConfigCommand : Command<GlobalOptions, MakeConfigOptions>
    {

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:make").Description("Create a config settings file");
        }
    }
}
