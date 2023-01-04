using MatthiWare.CommandLine.Core.Attributes;

namespace Bytehide.CLI.Models.Config
{
    public class MakeConfigOptions
    {
        [Name("p", "path"), Required, Description("Path of the protection config file")]
        public string Path { get; set; }
        [Name("t", "type"), Required, Description("Type of protection ( project or application")]
        public string Type { get; set; }
        [Name("n", "name"), Required, Description("Name of the protection config file")]
        public string Name { get; set; }

        [Name("ps", "preset"), Description("Choose a preset for the proteccion config file")]
        public string Preset { get; set; }

    }
}