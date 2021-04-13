using MatthiWare.CommandLine.Core.Attributes;
using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Models
{
    public class GetProjectOptions
    {
        [Name("", "name"), Description("Make a project"), OptionOrder(0)]
        public string Name { get; set; }
        [Name("k", "key"), Description("Get a project by key")]
        public string Key { get; set; }
        [Name("","or-create"),DefaultValue(true), Description("Create a project if not exists")]
        public bool Create { get; set; }

    }
}
