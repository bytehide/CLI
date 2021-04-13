using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;

namespace ShieldCLI.Commands.Config
{
    public class GetConfigCommand : Command<GlobalOptions, GetConfigOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:find").Description("Get a config from a path");
        }
    }
}
