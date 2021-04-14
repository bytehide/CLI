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
            if (!ClientManager.HasValidClient()) {

               AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
               return;
            };

            var project = ClientManager.Client.Project.FindByIdOrCreateExternalProject("probandoNombreID123","d713d01b-8001-4917-8ff2-c8fe961b25b7");


            Console.WriteLine("este es el nombre "+ project.Name);
        }




    
    }
}
