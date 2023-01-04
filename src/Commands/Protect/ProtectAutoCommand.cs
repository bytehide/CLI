using System;
using System.IO;
using System.Threading.Tasks;
using Bytehide.CLI.Helpers;
using Bytehide.CLI.Repos;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Commands.Protect
{
    internal class ProtectAutoCommand : AsyncCommand, ICommandLimiter<ShieldSettings>
    {
        public ShieldCommands ShieldCommands { get; }
        public ClientManager ClientManager { get; }

        public ProtectAutoCommand(ShieldCommands shieldCommands, ClientManager clientManager)
        {
            ShieldCommands = shieldCommands;
            ClientManager = clientManager;
        }

        public override async Task<int> ExecuteAsync(CommandContext context)
        {
            try
            {
                _ = ShieldCommands.AuthHasCredentials();

                var name = AnsiConsole.Ask<string>("[darkorange]Enter the project Name[/]");

                var project = await ShieldCommands.FindOrCreateProjectByNameAsync(name);

                ShieldCommands.PrintProject(project.Name, project.Key);
                AnsiConsole.MarkupLine("");
                var path = AnsiConsole.Ask<string>("[darkorange]Enter the path of the application:[/]");

                var appUpload = await ShieldCommands.UploadApplicationAsync(path, project.Key);

               

                ShieldCommands.PrintApplication(Path.GetFileName(path), appUpload.ApplicationBlob, project.Key);



                var directory = Path.GetDirectoryName(path);
                var filename = Path.GetFileNameWithoutExtension(path);
                var appName = Path.GetFileName(path);

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

                await ShieldCommands.ProtectApplicationAsync(project.Key, appUpload.ApplicationBlob, config, savePath);

                return 0;
            }
            catch (Exception ex)
            {
                ExceptionHelpers.ProcessException(ex);

                return 1;
            }
        }
    }
}
