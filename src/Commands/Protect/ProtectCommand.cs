using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Protect;
using ShieldCLI.Repos;
using Spectre.Console;
using Shield.Client.Extensions;
using System.Threading;
using System.IO;
using System.Threading.Tasks;
using Shield.Client.Models.API.Project;

namespace ShieldCLI.Commands.Protect
{
    public class ProtectCommand : Command<GlobalOptions, ProtectOptions>
    {

        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public ProtectCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("protect").Description("Protect an application or project.");



        }

        public override async Task OnExecuteAsync(GlobalOptions option, ProtectOptions options, CancellationToken cancellationToken)
        {
            var projectName = options.ProjectName;
            var projectKey = options.ProjectKey;
            var pathApp = options.PathApp;
            var configName = Path.GetFileName(options.Config);
            var configDirectory = Path.GetDirectoryName(options.Config);
            var pathOutput = options.Output;


            ProjectDto project = null;
            if (projectKey == "default" && projectName == "default")
            {
                AnsiConsole.MarkupLine("[red]Need a KEY or a NAME of a project to protect the application[/]");
                return;
            }

            if (projectName != "default")
            {
                project = await ShieldCommands.ProjectFindOrCreateByNameAsync(projectName);
                projectKey = project.Key;

            }



            if (Path.GetDirectoryName(options.Config) == "")
            {
                configName = $"Shield.Application.{options.Config}.json";
                configDirectory = Path.GetDirectoryName(pathApp);
            }


            var appUpload = await ShieldCommands.UploadApplicationAsync(pathApp, projectKey);



            var config = ShieldCommands.ConfigApplicationGetFile(configDirectory, configName, true);

            await ShieldCommands.ProtectApplicationAsync(projectKey, appUpload.ApplicationBlob, config, pathOutput);


        }
    }

}