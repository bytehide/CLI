using MatthiWare.CommandLine.Core.Attributes;

namespace Dotnetsafer.CLI.Models.Config
{
    public class GetConfigOptions
    {
        [Name("p", "path"), Required, Description("Path of the protection config file")]
        public string Path { get; set; }
        [Name("t", "type"), Required, Description("Type of protection ( project or application")]
        public string Type { get; set; }

        [Name("n", "name"), Required, Description("Name of the protection config file")]
        public string Name { get; set; }

        [Name("", "or-create"), DefaultValue(false), Description("Create a default config file")]
        public bool Create { get; set; }
    }
}