using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Protect
{
    public class OldProtectOptions : ShieldSettings
    {
        [Name("k", "keyproject"), Required, DefaultValue("default"), Description("Key of the project.")]
        public string ProjectKey { get; set; }

        [Name("n", "projectname"), Required, DefaultValue("default"), Description("Key of the project.")]
        public string ProjectName { get; set; }

        [Name("p", "path"), Required, Description("Path of the application.")]
        public string PathApp { get; set; }

        [Name("", "config"), Required, Description("Path of Config File.")]
        public string Config { get; set; }

        [Name("", "output"), Required, Description("Path of the protected file.")]
        public string Output { get; set; }

    }
}
