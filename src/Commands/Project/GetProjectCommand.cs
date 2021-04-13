using System;
using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Project
{
    public class GetProjectCommand : Command<GlobalOptions, GetProjectOptions>
    {
        public ClientManager ClientManager { get; set; }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:find").Description("Projects Management");
        }

        public GetProjectCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }

        public override void OnExecute(GlobalOptions options, GetProjectOptions getProjectOptions)
        {
            base.OnExecute(options, getProjectOptions);

            var name =  getProjectOptions.Name;
            var key = getProjectOptions.Key;
            var shouldCreated = getProjectOptions.Create;


            try

            {
                if (name == null && key == null)
                {
                    throw new ArgumentNullException();
                }


                if (key != null)
                {
                    Console.WriteLine("se busca por Key");
                    return;
                }


                
                var project = ClientManager.Client.Project.FindOrCreateExternalProject(name);

                AnsiConsole.Markup("[lime]Project Found [/]");
                Console.WriteLine("");

                var table = new Table();
                // Add some columns
                table.AddColumn("[darkorange]Name[/]");
                table.AddColumn("[darkorange]ID[/]");

                // Add some rows
                table.AddRow(project.Name, project.Key);

                // Render the table to the console
                AnsiConsole.Render(table);
            

            }
            catch (ArgumentNullException ex)
            {
                AnsiConsole.Markup("[red]Should insert the name or key of the project to find it.[/]");
                ;

            }







        }


    }
}
