using System.ComponentModel;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Models.Config
{


    internal class ConfigGetCommandSettings : Branches.ShieldSettings
    {
        [CommandArgument(1, "[NAME]"), Description("Name of the protection config file")]
        public string Name { get; set; }
        [CommandArgument(0, "<PATH>"), Description("Path of the protection config file")]
        public string Path { get; set; }

        [CommandOption("--create"), Description("Create a config file if not exist."), DefaultValue(false)]
        public bool Create { get; set; }
    }
}