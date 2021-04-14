using MatthiWare.CommandLine.Core.Attributes;

namespace ShieldCLI.Models.Auth
{
    public class AuthOptions
    {
        [Name("r", "register"), DefaultValue(false), Description("Register user in Shield")]
        public bool Register { get; set; }

        [Name("l","login"), Description("API Token to login in Shield")]
        public string Login { get; set; }

        [Name("cl", "clear"),DefaultValue(false), Description("Clear stored credentials")]
        public bool Clear { get; set; }

        [Name("ch", "check"), DefaultValue(false), Description("Start session with stored credentials"), OptionOrder(0)]
        public bool Check { get; set; }
    }
}
