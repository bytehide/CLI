using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Config
{
    public class GetConfigCommand : Command<GlobalOptions, GetConfigOptions>

    {

        private ClientManager ClientManager { get; set; }

        public GetConfigCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:find").Description("Get a config from a path");
        }

        public override void OnExecute(GlobalOptions option, GetConfigOptions options)
        {



            if (!ClientManager.HasValidClient())
            {

                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };

            try
            {
                var type = options.Type;
                var path = options.Path;
                var name = "*.";


                if (options.Name != null)
                {

                    name = $"{options.Name}.";
                }

                if (type != "application" && type != "project")
                {
                    type = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                             .Title("[white]Please choose the type of protection[/]?")
                             .PageSize(3)
                             .AddChoice("project")
                             .AddChoice("application"));
                }
                var fullFilePath = $"{path}/shield.{type}.{name}json";

                //TODO check this file JSON . 
            }
            catch
            {


            }


        }
    }
}
