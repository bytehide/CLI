using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("application:add").Description("Add an application to a project");
        }

    }
}