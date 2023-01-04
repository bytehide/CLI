using MatthiWare.CommandLine.Core.Attributes;

namespace Bytehide.CLI.Models.Project
{
    public class GetProjectOptions
    {
        [Name("", "name"), Description("Get a project by name"), OptionOrder(0)]
        public string Name { get; set; }
        [Name("k", "key"), Description("Get a project by key")]
        public string Key { get; set; }
        [Name("", "or-create"), DefaultValue(false), Description("Create a project if not exists")]
        public bool Create { get; set; }

    }
}
