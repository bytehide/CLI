using MatthiWare.CommandLine.Core.Attributes;
using ShieldCLI.Commands;
using ShieldCLI.Commands.Config;

namespace ShieldCLI.Models

{
    public class GlobalOptions
    {
      


        [Name("v", "verbose"), DefaultValue(false), Description("Verbose output")]
        public bool Verbose { get; set; }


        //En vez de utilizar discover(que los ordena por orden alfabetico puedo usar esta manera que los ordeno yo. 
        //public DeleteProjectCommand DeleteProjectCommand { get; set; }
        
        public AuthCommand AuthCommand { get; set; }
        public ListProjectsCommand ListProjectsCommand { get; set; }

        public CreateProjectCommand CreateProjectCommand { get; set; }
        public GetProjectCommand GetProjectCommand { get; set; }
        public DeleteProjectCommand DeleteProjectCommand { get; set; }
        public GetConfigCommand GetConfigCommand { get; set; }
        public MakeConfigCommand MakeConfigCommand { get; set; }
        public ProtectCommand ProtectCommand { get; set; }


    }
}