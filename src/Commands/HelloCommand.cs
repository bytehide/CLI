using System.Reflection;
using Spectre.Console;
using Spectre.Console.Cli;


namespace Dotnetsafer.CLI.Commands
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
        [blue]Welcome to the dotnetsafer console, keep your .NET software safe with a few commands!
    __________________
                      \
                       \

  _____        _              _              __             _____ _      _____ 
 |  __ \      | |            | |            / _|           / ____| |    |_   _|
 | |  | | ___ | |_ _ __   ___| |_ ___  __ _| |_ ___ _ __  | |    | |      | |  
 | |  | |/ _ \| __| '_ \ / _ \ __/ __|/ _` |  _/ _ \ '__| | |    | |      | |  
 | |__| | (_) | |_| | | |  __/ |_\__ \ (_| | ||  __/ |    | |____| |____ _| |_ 
 |_____/ \___/ \__|_| |_|\___|\__|___/\__,_|_| \___|_|     \_____|______|_____|
             [/]");

            var table = new Table();
            table.AddColumn("Website");
            table.AddColumn("Documentation");
            table.AddColumn("Version");
            table.AddRow("[link=https://dotnetsafer.com]https://dotnetsafer.com[/]", "[link=https://dotnetsafer.com/docs/cli]https://dotnetsafer.com/docs/cli[/]", CurrentVersion);
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
[lime] > dotnetsafer shield --help[/] to discover the options.
");
            return 0;
        }
    }
}
