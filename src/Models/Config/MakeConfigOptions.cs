using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Config
{
    public class MakeConfigOptions
    {
        [Name("d", "directory"), Required, Description("Path of the protection config file")]
        public string Path { get; set; }
        [Name("t", "type"), Required, Description("Type of protection ( project or application")]
        public string Type { get; set; }
        [Name("f", "file"), Required, Description("Name of the protection config file")]
        public string File { get; set; }
        [Name("p", "preset"), Description("Choose a preset for the proteccion config file")]
        public string Preset { get; set; }

    }
}