using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Commands;
using ShieldCLI.Models;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using Spectre.Console;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.App
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {
        private ClientManager ClientManager { get; set; }

        private ShieldCommands ShieldCommands { get; set; }

        public AddAppCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
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


            //try
            //{
            //    var keyproject = options.KeyProject;
            //    var path = options.Path;
            //    List<string> dependenciesPaths = Directory.GetFiles(Path.GetDirectoryName(path)).ToList();

            //    ShieldCommands.saludoShield();

            //    var appUpload = ClientManager.Client.Application.UploadApplicationDirectly(keyproject, path, dependenciesPaths);

            //}
            //catch
            //{
            //    AnsiConsole.Write("Error");
            //}




        }

        //TODO: @jespanag  Create method to get projects on Client
    }

}
