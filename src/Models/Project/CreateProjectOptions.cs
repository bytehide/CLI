using MatthiWare.CommandLine.Core.Attributes;


namespace ShieldCLI.Models

{
    public class CreateProjectOptions
    {

        [Name("","name"), Required, Description("Make a project"), OptionOrder(0)]
        public string Name { get; set; }
    }
}