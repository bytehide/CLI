using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Repos;

namespace ShieldCLI.Commands.Config
{
    public class GetConfigCommand : Command<GlobalOptions, GetConfigOptions>
    {
        public ShieldCommands ShieldCommands { get; set; }

        public GetConfigCommand(ShieldCommands shieldCommands)
        {
            ShieldCommands = shieldCommands;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:find").Description("Get a config from a path");
        }
        //TODO:
        //public override void OnExecute(GlobalOptions option, GetConfigOptions options)
        //{
        //    ShieldCommands.AuthHasCredentials();



        //    var type = ShieldCommands.ChooseType(options.Type);


        //    if (type == "project")
        //    {
        //        ProjectConfigurationDto config = ShieldCommands.ConfigProjectGetFile(options.Path, options.Name, options.Create);

        //    }
        //    else
        //    {
        //        ApplicationConfigurationDto config = ShieldCommands.ConfigApplicationGetFile(options.Path, options.Name, options.Create);
        //    }





        //}
    }
}
