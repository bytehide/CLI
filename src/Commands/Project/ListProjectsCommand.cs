using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class ListProjectsCommand : Command<GlobalOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:list").Description("List all projects");
        }
        

        


    }
}
