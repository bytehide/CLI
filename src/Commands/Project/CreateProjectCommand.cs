using MatthiWare.CommandLine.Abstractions.Command;
using MatthiWare.CommandLine.Abstractions.Parsing;
using Shield.Client;
using ShieldCLI.Models;
using ShieldCLI.Repos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class CreateProjectCommand : Command<GlobalOptions, CreateProjectOptions>
    {
        private ShieldClient ShieldClient { get; set; }
        private KeyManager KeyManager { get; set; }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:make").Description("Create project");
        }

        public CreateProjectCommand(ShieldClient shieldClient, KeyManager keyManager)
        {
            ShieldClient = shieldClient;
            KeyManager = keyManager;
        }

        public override void OnExecute(GlobalOptions options, CreateProjectOptions createoptions)
        {
            base.OnExecute(options, createoptions);

            try
            {
                var createdProject= ShieldClient.Project.FindOrCreateExternalProject(createoptions.Name);
                AnsiConsole.Markup("[lime]Project " + createdProject.Name + " created. [/]");
                Console.WriteLine();
                var table = new Table();

                // Add some columns
                table.AddColumn("Key");
                table.AddColumn("Name");

                // Add some rows
                table.AddRow(createdProject.Key, createdProject.Name);
             

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
