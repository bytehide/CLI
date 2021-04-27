using Spectre.Console.Cli;
using System.ComponentModel;

namespace ShieldCLI.Commands.Config
{


    internal class ConfigGetCommandSettings : Branches.ShieldSettings
    {
        [CommandArgument(0, "<NAME>"), Description("Name of the protection config file")]
        public string Name { get; set; }
        [CommandArgument(1, "<PATH>"), Description("Path of the protection config file")]
        public string Path { get; set; }

        [CommandArgument(2, "[TYPE]"), Description("Type of the protection config file."), DefaultValue("project")]
        public string Type { get; set; }

        [CommandOption("--create"), Description("Create a config file if not exist."), DefaultValue(false)]
        public bool Create { get; set; }
    }










    //[Name("", "or-create"), DefaultValue(false), Description("Create a default config file")]
    //public bool Create { get; set; }
}