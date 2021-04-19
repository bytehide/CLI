using ShieldCLI.Models;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;
using System;
using Spectre.Console;

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

        public override async Task OnExecuteAsync(GlobalOptions option, AddAppOptions options, CancellationToken cancellationToken)
        {
            ShieldCommands.AuthHasCredentials();

            await ShieldCommands.UploadApplicationAsync(options.Path, options.KeyProject);


            //var dependencies = await ShieldCommands.ResolveDependenciesAsync(options.Path);

            //await ClientManager.Client.Application.UploadApplicationDirectlyAsync(options.KeyProject,
            //    options.Path, dependencies.Select(dep => dep.Item2).ToList());

        }

        //public override void OnExecute(GlobalOptions option, AddAppOptions options)
        //{
        //   

        //    try
        //    {
        //        var run = Task.Run(async () =>
        //        {
        //             var dependencies = await ShieldCommands.ResolveDependenciesAsync(options.Path);

        //             return await ClientManager.Client.Application.UploadApplicationDirectlyAsync(options.KeyProject,
        //                 options.Path, dependencies.Select(dep => dep.Item2).ToList());
        //        });

        //        var resolved = AsyncHelpers.RunSync(() => Task.FromResult(run));
        //    }
        //    catch (Exception ex)
        //    {
        //        AnsiConsole.WriteException(ex);
        //    }

        //}
    }
}
