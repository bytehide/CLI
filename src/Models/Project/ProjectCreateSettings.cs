using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI.Models.Project
{
    internal class ProjectCreateCommandSettings : Branches.ShieldSettings
    {
        [CommandArgument(0, "<PROJECT>"), Description("Name of the dotnetsafer shield project that will be create.")]
        public string Project { get; set; }

        public override ValidationResult Validate()
        {
            return Project.Length < 3
            ? ValidationResult.Error("Name must be at least three characters long")
            : ValidationResult.Success();
        }
    }
}
