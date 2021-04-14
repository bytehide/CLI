using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;
using ShieldCLI.Repos;
using Spectre.Console;
using System;

namespace ShieldCLI.Commands.Project
{
    public class ListProjectsCommand : Command<GlobalOptions>
    {
        private ClientManager ClientManager { get; set; }

        public ListProjectsCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:list").Description("List all projects");
        }


        public override void OnExecute(GlobalOptions options)
        {


            if (!ClientManager.HasValidClient())
            {

                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };

            //TODO: @jespanag  Create method to get projects on +Client
        }





    }
}
