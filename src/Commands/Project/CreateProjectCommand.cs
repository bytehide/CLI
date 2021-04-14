using System;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Project;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Project
{
    public class CreateProjectCommand : Command<GlobalOptions, CreateProjectOptions>
    {
        private ClientManager ClientManager { get; set; }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:make").Description("Create project");
        }

        public CreateProjectCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }

        public override void OnExecute(GlobalOptions options, CreateProjectOptions createOptions)
        {
            base.OnExecute(options, createOptions);

            try
            {
                if (!ClientManager.HasValidClient())
                {

                    AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                    return;
                };

                var createdProject = ClientManager.Client.Project.FindOrCreateExternalProject(createOptions.Name);

                AnsiConsole.Markup("[lime]Project " + createdProject.Name + " created. [/]");
                Console.WriteLine();
                var table = new Table();

                // Add some columns
                table.AddColumn("Name");
                table.AddColumn("Key");

                // Add some rows
                table.AddRow(createdProject.Name, createdProject.Key);
             

                // Render the table to the console
                AnsiConsole.Render(table);


            }
            catch (Exception ex)
            {
                //AnsiConsole.Markup("[red]"+ ex.Message +"[/]");
            }
            
        }
    }
}
