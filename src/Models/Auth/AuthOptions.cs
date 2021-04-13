using MatthiWare.CommandLine.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Models
{
    public class AuthOptions
    {
        [Name("r", "register"), DefaultValue(true), Description("Register user in Shield"),OptionOrder(0)]
        public bool Register { get; set; }

        [Name("l","login"), Description("API Token to login in Shield")]
        public string Login { get; set; }

        [Name("cl", "clear"),DefaultValue(true), Description("Clear stored credentials")]
        public bool Clear { get; set; }

        [Name("ch", "check"), DefaultValue(true), Description("Start session with stored credentials")]
        public bool Check { get; set; }
    }
}
