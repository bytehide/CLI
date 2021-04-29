using System.Threading;
using System.Threading.Tasks;
using Dotnetsafer.CLI.Models;
using Dotnetsafer.CLI.Models.App;
using MatthiWare.CommandLine.Abstractions.Command;

namespace Dotnetsafer.CLI.Commands.App
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {

        private ShieldCommands ShieldCommands { get; }

        public AddAppCommand(ShieldCommands shieldCommands)

        => ShieldCommands = shieldCommands;

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
