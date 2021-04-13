using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands
{
    public class GetProjectCommand : Command<GlobalOptions, GetProjectOptions>
    {
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("project:find").Description("Projects Management");
        }

        public override void OnExecute(GlobalOptions options, GetProjectOptions getoptions)
        {
            base.OnExecute(options, getoptions);

            var nombre = getoptions.Name;
            var ke = getoptions.Key;
            var create = getoptions.Create;

            if (nombre !=null)
            {


                Console.WriteLine("funciona y el nombre es");

                Console.WriteLine(nombre);
            }
            else if (nombre==null && create)
            {
                Console.WriteLine("se crea proyecto");

            }
            else
            {

                Console.WriteLine("funciona y la key es");
                Console.WriteLine(ke);


            }





        }


    }
}
