using MatthiWare.CommandLine.Core.Attributes;

namespace Dotnetsafer.CLI.Models.App
{
    public class AddAppOptions
    {

        [Name("k", "keyProject"), Required, Description("Key of the project where app is added.")]
        public string KeyProject { get; set; }

        [Name("p", "path"), Required, Description("Path of the application.")]
        public string Path { get; set; }

        //[Name("ad", "auto-discover"), DefaultValue(true), Required, Description("Auto-check dependencies needed and find them in the current directory.")]
        //public bool discover { get; set; }
    }
}