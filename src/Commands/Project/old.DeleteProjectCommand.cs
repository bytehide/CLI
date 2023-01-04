using Bytehide.CLI.Models;
using MatthiWare.CommandLine.Abstractions.Command;

namespace Bytehide.CLI.Commands.Project
{
    public class DeleteProjectCommand : Command<GlobalOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:delete").Description("Delete project");
        }


        //TODO @Sr-l  method to delete projects in client
    }

}

