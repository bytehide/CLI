using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Models;
using ShieldCLI.Models.Protect;
using ShieldCLI.Repos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.Protect
{
    public class ProtectAuto : Command<GlobalOptions, ProtectAutoOptions>
    {
        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public ProtectAuto(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect:auto").Description("Protect an application.");



        }

        public override async Task OnExecuteAsync(GlobalOptions option, ProtectAutoOptions options, CancellationToken cancellationToken)
        {


            ShieldCommands.AuthHasCredentials();

            string name = AnsiConsole.Ask<string>("[darkorange]Enter the project Name[/]");

            var project = await ShieldCommands.ProjectFindOrCreateByNameAsync(name);

            ShieldCommands.ProjectTable(project.Name, project.Key);

            string path = AnsiConsole.Ask<string>("[darkorange]Enter the path of the application:[/]");



            var appUpload = await ShieldCommands.UploadApplicationAsync(path, project.Key);


            Console.WriteLine("");
            AnsiConsole.MarkupLine("[lime]Application Uploaded Succesfully[/]");

            ShieldCommands.ApplicationtTable(Path.GetFileName(path), appUpload.ApplicationBlob, project.Key);

            var directory = Path.GetDirectoryName(path);
            var filename = Path.GetFileNameWithoutExtension(path);

            var autoconfig = ClientManager.Client.Configuration.FindApplicationConfiguration(directory, filename);

            ApplicationConfigurationDto config = null;

            if (autoconfig is not null)
            {
                AnsiConsole.MarkupLine("[darkorange]We detected an application config file[/]");
                if (!AnsiConsole.Confirm("Do you want to use it? "))
                {
                    config = ShieldCommands.CreateConfigFile(project.Key, directory);
                }

                config = autoconfig;

            }
            else
            {
                config = ShieldCommands.CreateConfigFile(project.Key, directory);
            }


            await ShieldCommands.ProtectApplicationAsync(project.Key, appUpload.ApplicationBlob, config);

        }












        //var projectKey = options.ProjectKey;
        //var appKey = options.AppKey;
        //var config = options.Config;
        //var output = options.Output;

        //var appConfig = ClientManager.Client.Configuration.LoadApplicationConfigurationFromFile(config);




        //var connection = ClientManager.Client.Connector.CreateHubConnection();
        //var hub = ClientManager.Client.Connector.InstanceHubConnectorAsync(connection).Result;

        //hub.StartAsync().Wait();

        //var result = ClientManager.Client.Tasks.ProtectSingleFile("projectKety", "appKey", connection, appConfig);



        //result.OnSuccess(hub, (a) =>
        //{

        //    AnsiConsole.Markup($"[lime]{a.Name} application has been protected SUCESSFULLY. [/]");
        //}
        //);

        //result.OnError(hub, AnsiConsole.Write);
        //result.OnClose(hub, (s) =>
        //{
        //    AnsiConsole.Markup($"[lime]{s} [/]");
        //});
    }
}



