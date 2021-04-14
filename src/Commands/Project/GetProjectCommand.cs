using System;
using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client;
using Shield.Client.Models.API.Project;
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

            try

            {
                var name = getProjectOptions.Name;
                var key = getProjectOptions.Key;
                ProjectDto project = null;

                if (name == null && key == null)
                {
                    throw new ArgumentNullException();
                }


                if (key != null)
                {
                    project = ClientManager.Client.Project.FindByIdOrCreateExternalProject(name ?? "default", key);
                    AnsiConsole.Markup("[lime]Project Found [/]");
                }
                else
                {
                    project = ClientManager.Client.Project.FindOrCreateExternalProject(name);
                    AnsiConsole.Markup("[lime]Project Found [/]");
                }

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
