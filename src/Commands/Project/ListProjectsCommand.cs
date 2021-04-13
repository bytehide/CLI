using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;

namespace ShieldCLI.Commands.Project
{
    public class ListProjectsCommand : Command<GlobalOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:list").Description("List all projects");
        }


        public override void OnExecute(GlobalOptions options)
        {

           
        }




    }
}
