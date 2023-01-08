using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;


namespace Bytehide.CLI.Commands
{
    internal class HelloCommand : Command, ICommandLimiter<ShieldSettings>
    {
        public string CurrentVersion =>
            //ApplicationDeployment.IsNetworkDeployed
            //    ? ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString()
            //    :
            Assembly.GetExecutingAssembly().GetName().Version?.ToString();

        public override int Execute(CommandContext context)
        {
            AnsiConsole.MarkupLine(
                @"
        [blue]Welcome to the ByteHide console, keep your .NET software safe with a few commands!
    __________________
                      \
                       \
  ____        _       _    _ _     _         _____ _      _____ 
 |  _ \      | |     | |  | (_)   | |       / ____| |    |_   _|
 | |_) |_   _| |_ ___| |__| |_  __| | ___  | |    | |      | |  
 |  _ <| | | | __/ _ \  __  | |/ _` |/ _ \ | |    | |      | |  
 | |_) | |_| | ||  __/ |  | | | (_| |  __/ | |____| |____ _| |_ 
 |____/ \__, |\__\___|_|  |_|_|\__,_|\___|  \_____|______|_____|
         __/ |                                                  
        |___/      
             [/]");

            var table = new Table();
            table.AddColumn("Website");
            table.AddColumn("Documentation");
            table.AddColumn("Version");
            table.AddRow("[link=https://bytehide.com]https://bytehide.com[/]", "[link=https://docs.bytehide.com/platforms/dotnet/products/shield/cli-quick-start]https://docs.bytehide.com/platforms/dotnet/products/shield/cli-quick-start[/]", CurrentVersion);
            AnsiConsole.Render(table);

            AnsiConsole.MarkupLine(@"

[lime] > NEW ![/] Try Shield from this CLI [darkorange](Beta)[/]:
[violet]
           @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@          
     @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@    
  /@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@  
 @@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@@
 @@@@@@@@@@@@@@@@@@@@@@@@@*********@@@@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@@////@@@@@@@****@@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@///@@@@@@@@@@@***@@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@#//@@@@@@@@@@@@@**&@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@#//@@@@@@@@@@@@@**&@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@#//@@@@@@@@@@@@@@*&@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@@@@@///////////*******@@@@@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@#/////////////////********#@@@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@///////////@@@/////*******(##@@@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@&///////@@@@////////******#####@@@@@@@@@@@@
@@@@@@@@@@@@@@@@@@/////@@@@////@@@@////****#######@@@@@@@@@@
@@@@@@@@@@@@@@@@@@///////@@@@////@@@@///***#########@@@@@@@@
@@@@@@@@@@@@@@@@@@@(///////@@@///@@@@////*######%#####&@@@@@
@@@@@@@@@@@@@@@@@@@@(/////////@@@@(//////###############%@@@
@@@@@@@@@@@@@@@@@@@@@(/////////@////////###################@
@@@@@@@@@@@@@@@@@@@@@@@@/////////////#######################
 @@@@@@@@@@@@@@@@@@@@@@@@@#///////##########################
 @@@@@@@@@@@@@@@@@@@@@@@@@@@################################
  .@@@@@@@@@@@@@@@@@@@@@@@@@@@############################  
     @@@@@@@@@@@@@@@@@@@@@@@@@@@########################  [/]
            
[blue]Use:[/]
[lime] > bytehide shield --help[/] to discover the options.
");
            return 0;
        }
    }
}
