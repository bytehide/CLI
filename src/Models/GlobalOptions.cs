using Dotnetsafer.CLI.Commands.Auth;
using Dotnetsafer.CLI.Commands.Protect;

namespace Dotnetsafer.CLI.Models

{
    //TODO: REMOVE
    public class GlobalOptions
    {

        //[Name("v", "verbose"), DefaultValue(false), Description("Verbose output")]
        //public bool Verbose { get; set; }


        //En vez de utilizar discover(que los ordena por orden alfabetico puedo usar esta manera que los ordeno yo. 
        //public DeleteProjectCommand DeleteProjectCommand { get; set; }
        public OldAuthCommand AuthCommand { get; set; }
        //public ListProjectsCommand ListProjectsCommand { get; set; }

        //public CreateProjectCommand CreateProjectCommand { get; set; }
        //public GetProjectCommand GetProjectCommand { get; set; }
        //public DeleteProjectCommand DeleteProjectCommand { get; set; }
        //public AddAppCommand AddAppCommand { get; set; }
        //public GetConfigCommand GetConfigCommand { get; set; }
        //public MakeConfigCommand MakeConfigCommand { get; set; }
        public OldProtectCommand ProtectCommand { get; set; }
        public ProtectAuto protectAuto { get; set; }


    }
}