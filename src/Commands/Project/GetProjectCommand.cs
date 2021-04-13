using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client;
using ShieldCLI.Models;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class GetProjectCommand : Command<GlobalOptions, GetProjectOptions>
    {
        public ShieldClient ShieldClient { get; set; }
        public object Ansiconsole { get; private set; }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:find").Description("Projects Management");
        }

        public GetProjectCommand(ShieldClient shieldClient)
        {
            ShieldClient = shieldClient;
        }

        public override void OnExecute(GlobalOptions options, GetProjectOptions getprojectoptions)
        {
            base.OnExecute(options, getprojectoptions);

            var name =  getprojectoptions.Name;
            var key = getprojectoptions.Key;
            var shouldCreated = getprojectoptions.Create;


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


                if (name != null)
                {
                    var project = ShieldClient.Project.FindOrCreateExternalProject(name);

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

            }
            catch (ArgumentNullException ex)
            {
                AnsiConsole.Markup("[red]Should insert the name or key of the project to find it.[/]");
                ;

            }







        }


    }
}
