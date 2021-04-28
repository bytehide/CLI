using System.ComponentModel;
using System.IO;
using Spectre.Console;
using Spectre.Console.Cli;

namespace ShieldCLI.Models.Protect
{
    internal class ProtectCommandSettings : Branches.ShieldSettings
    {
        [CommandArgument(0, "<PROJECT>"), Description("Name of the dotnetsafer shield project or Key if flag --key is true.")]
        public string Project { get; set; }

        [CommandArgument(1, "<APPLICATION PATH>"), Description("Path of the application.")]
        public string ApplicationPath { get; set; }

        [CommandArgument(2, "<CONFIGURATION PATH>"), Description("Path of the application shield configuration.")]
        public string ConfigurationPath { get; set; }

        [CommandArgument(3, "<OUTPUT DIRECTORY PATH>"), Description("Path of the output application protected.")]
        public string OutputPath { get; set; }

        [CommandOption("--key|-k"), Description("Set true if <PROJECT> is the key of the project."), DefaultValue(false)]
        public bool IsProjectKey { get; set; }

        public override ValidationResult Validate()
        {
            if (!File.Exists(ApplicationPath))
                return ValidationResult.Error("The path provided for the application is invalid.");
            return !File.Exists(ConfigurationPath) ?
                ValidationResult.Error("The path provided for the shield application configuration is invalid.")
                : ValidationResult.Success();
        }
    }
}
