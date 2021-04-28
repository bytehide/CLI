using Spectre.Console;
using Spectre.Console.Cli;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Models.App
{
    internal class AppAddCommandSettings : Branches.ShieldSettings
    {
        [CommandArgument(0, "<PROJECT>"), Description("Name of the dotnetsafer shield project or Key if flag --key is true.")]
        public string Project { get; set; }

        [CommandArgument(1, "<APPLICATION PATH>"), Description("Full path of the application that will be uploaded.")]
        public string ApplicationPath { get; set; }

        [CommandOption("--key|-k"), Description("Set true if <PROJECT> is the key of the project."), DefaultValue(false)]
        public bool IsProjectKey { get; set; }

        public override ValidationResult Validate()
        {
            if (!File.Exists(ApplicationPath))
                return ValidationResult.Error("The path provided for the application is invalid.");
            return ValidationResult.Success();
        }
    }
}
