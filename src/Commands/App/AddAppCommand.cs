using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ShieldCLI.Commands.App
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {
        private ClientManager ClientManager { get; set; }

        public AddAppCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("application:add").Description("Add an application to a project");
        }


        public override void OnExecute(GlobalOptions option, AddAppOptions options)
        {


            if (!ClientManager.HasValidClient())
            {

                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };

            try
            {
                var keyproject = options.KeyProject;
                var path = options.Path;
                List<string> dependenciesPaths = Directory.GetFiles(Path.GetDirectoryName(path)).ToList();

                var appUpload = ClientManager.Client.Application.UploadApplicationDirectly(keyproject, path, dependenciesPaths);

            }
            catch
            {
                AnsiConsole.Write("Errora sdfasdf");
            }

        }
        //TODO: @jespanag  Create method to get projects on Client
    }

}
