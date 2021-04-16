using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Auth
{
    public class AuthOptions
    {
        [Name("r", "register"), DefaultValue(false), Description("Register user in Shield")]
        public bool Register { get; set; }

        [Name("l", "login"), Description("Login in Shield with an API token"), OptionOrder(0)]
        public string Login { get; set; }

        [Name("cl", "clear"), DefaultValue(false), Description("Clear stored credentials")]
        public bool Clear { get; set; }

        [Name("ch", "check"), DefaultValue(false), Description("Start session with stored credentials")]
        public bool Check { get; set; }
    }
}
