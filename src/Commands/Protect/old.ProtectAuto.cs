using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Dotnetsafer.CLI.Helpers;
using Dotnetsafer.CLI.Models;
using Dotnetsafer.CLI.Models.Protect;
using Dotnetsafer.CLI.Repos;
using MatthiWare.CommandLine.Abstractions.Command;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI.Commands.Protect
{
    public class ProtectAuto : Command<GlobalOptions, OldProtectAutoOptions>
    {
        private ClientManager ClientManager { get; set; }
        public ShieldCommands ShieldCommands { get; set; }

        public ProtectAuto(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)

        => builder.Name("protect:auto").Description("Protect an application.");


        public override async Task OnExecuteAsync(GlobalOptions option, OldProtectAutoOptions options, CancellationToken cancellationToken)
        {
            try
            {
                ShieldCommands.AuthHasCredentials();

                var name = AnsiConsole.Ask<string>("[darkorange]Enter the project Name[/]");

                var project = await ShieldCommands.FindOrCreateProjectByNameAsync(name);

                ShieldCommands.PrintProject(project.Name, project.Key);
                AnsiConsole.MarkupLine("");
                var path = AnsiConsole.Ask<string>("[darkorange]Enter the path of the application:[/]");

                var appUpload = await ShieldCommands.UploadApplicationAsync(path, project.Key);

                Console.WriteLine("");
                AnsiConsole.MarkupLine("[lime]Application Uploaded Succesfully[/]");

                ShieldCommands.PrintApplication(Path.GetFileName(path), appUpload.ApplicationBlob, project.Key);

                var directory = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);

                var config = ClientManager.Client.Configuration.FindApplicationConfiguration(directory, filename);


                if (config is not null)
                {
                    AnsiConsole.MarkupLine("[darkorange]We detected an application config file[/]");
                    if (!AnsiConsole.Confirm("Do you want to use it? "))
                        config = ShieldCommands.CreateConfigurationFile(project.Key, directory, filename);

                }
                else
                {
                    config = ShieldCommands.CreateConfigurationFile(project.Key, directory, filename);
                }

                AnsiConsole.WriteLine("");

                var savePath = AnsiConsole.Ask<string>("[darkorange]Enter a path where protected app will be saved[/]");

               // await ShieldCommands.ProtectApplicationAsync(project.Key, appUpload.ApplicationBlob, config, savePath);
            }
            catch (Exception ex)
            {
                ExceptionHelpers.ProcessException(ex);
            }
        }

        public ValidationResult Validate(CommandContext context, CommandSettings settings)
        {
            throw new NotImplementedException();
        }

        public Task<int> Execute(CommandContext context, CommandSettings settings)
        {
            throw new NotImplementedException();
        }
    }
}



