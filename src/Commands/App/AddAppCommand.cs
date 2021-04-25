using ShieldCLI.Models;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using System.Threading;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions.Command;

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

        }
    }
}
