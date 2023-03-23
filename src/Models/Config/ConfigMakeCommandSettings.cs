using System.ComponentModel;
using Spectre.Console.Cli;

namespace Bytehide.CLI.Models.Config
{
    internal class ConfigMakeCommandSettings : Branches.ShieldSettings
    {

        [CommandArgument(0, "<NAME>"), Description("Name of the Shield protection config file")]
        public string Name { get; set; }
        [CommandArgument(1, "<DIRECTORY PATH>"), Description("Output direcroty path where configuration file will be saved.")]
        public string Path { get; set; }
        [CommandArgument(2, "[TYPE]"), Description("Type of the protection config file."), DefaultValue("application")]
        public string Type { get; set; }
        [CommandArgument(3, "[PRESET]"), Description("Shield preset for the protection config file."), DefaultValue("balance")]
        public string Preset { get; set; }

    }
}