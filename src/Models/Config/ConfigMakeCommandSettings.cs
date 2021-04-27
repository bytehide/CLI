using Spectre.Console.Cli;
using System.ComponentModel;

namespace ShieldCLI
{
    internal class ConfigMakeCommandSettings : Branches.ShieldSettings
    {

        [CommandArgument(0, "<NAME>"), Description("Name of the Shield protection config file")]
        public string Name { get; set; }
        [CommandArgument(1, "<PATH>"), Description("Path of the  file")]
        public string Path { get; set; }
        [CommandArgument(2, "[TYPE]"), Description("Type of the protection config file."), DefaultValue("project")]
        public string Type { get; set; }
        [CommandArgument(2, "[PRESET]"), Description("Shield preset for the protection config file."), DefaultValue("project")]
        public string Preset { get; set; }


    }
}