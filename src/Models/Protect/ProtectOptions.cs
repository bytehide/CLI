using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Protect
{
    public class ProtectOptions
    {
        [Name("projectkey"), Required, Description("Key of the project.")]
        public string ProjectKey { get; set; }

        [Name("appkey"), Description("Application Key that will be protected")]
        public string AppKey { get; set; }

        [Name("config"), Required, Description("Path of Config File")]
        public string Config { get; set; }

        [Name("output"), Required, Description("Path of the protected file")]
        public string Output { get; set; }

        [Name("auto"), DefaultValue(false), Description("Automatic protection for your files.")]
        public string Auto { get; set; }

    }
}
