using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Config
{
    public class GetConfigOptions
    {
        [Name("p", "path"), Description("Path of the protection config file")]
        public string Path { get; set; }
        [Name("t", "type"), Description("Type of protection ( project or application")]
        public string Type { get; set; }

        [Name("n", "name"), Description("Name of the protection config file")]
        public string Name { get; set; }

        [Name("", "default"), DefaultValue(true), Description("Create a default config file")]
        public bool Def { get; set; }
    }
}