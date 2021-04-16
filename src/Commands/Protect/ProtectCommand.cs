using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Models.Protect;
using ShieldCLI.Repos;
using Spectre.Console;
using Shield.Client.Extensions;
using ShieldCLI.Commands.Project;

namespace ShieldCLI.Commands.Protect
{
    public class ProtectCommand : Command<GlobalOptions, ProtectOptions>
    {

        private ClientManager ClientManager { get; set; }

        public ProtectCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect").Description("Protect an application or project.");



        }

        public override void OnExecute(GlobalOptions option, ProtectOptions options)
        {



            if (!ClientManager.HasValidClient())
            {

                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };


            var projectKey = options.ProjectKey;
            var appKey = options.AppKey;
            var config = options.Config;
            var output = options.Output;

            var appConfig = ClientManager.Client.Configuration.LoadApplicationConfigurationFromFile(config);




            var connection = ClientManager.Client.Connector.CreateHubConnection();
            var hub = ClientManager.Client.Connector.InstanceHubConnectorAsync(connection).Result;

            hub.StartAsync().Wait();

            var result = ClientManager.Client.Tasks.ProtectSingleFile("projectKety", "appKey", connection, appConfig);



            result.OnSuccess(hub, (a) =>
            {

                AnsiConsole.Markup($"[lime]{a.Name} application has been protected SUCESSFULLY. [/]");
            }
            );

            result.OnError(hub, AnsiConsole.Write);
            result.OnClose(hub, (s) =>
            {
                AnsiConsole.Markup($"[lime]{s} [/]");
            });
        }
    }

}