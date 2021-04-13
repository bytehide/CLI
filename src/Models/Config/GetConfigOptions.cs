using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Commands
{
    public class GetConfigOptions
    {
        [Name("d", "directory"), Required, Description("Path of the protection config file")]
        public string Path { get; set; }
        [Name("t", "type"), Required, Description("Type of protection ( project or application")]
        public string Type { get; set; }

        [Name("f", "file"), Description("Name of the protection config file")]
        public string File { get; set; }

        [Name("","default"),DefaultValue(true), Description("Create a default config file")]
        public bool Def { get; set; }
    }
}