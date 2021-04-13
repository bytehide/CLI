using MatthiWare.CommandLine.Abstractions.Command;
using MatthiWare.CommandLine.Abstractions.Parsing;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class CreateProjectCommand : Command<GlobalOptions, CreateProjectOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:make").Description("Create project");
        }

        public override void OnExecute(GlobalOptions options, CreateProjectOptions makeoptions)
        {
            base.OnExecute(options, makeoptions);

            var nombre = makeoptions.Name;

            Console.WriteLine("funciona y el nombre es");
            Console.WriteLine(nombre);

        }

        //private readonly IArgumentResolver<CreateProjectCommand> argResolver;
        //public CreateProjectCommand(IArgumentResolver<CreateProjectCommand> argResolver) { this.argResolver = argResolver; }

    }
}
