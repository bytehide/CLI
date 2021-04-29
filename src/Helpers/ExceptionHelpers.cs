using System;
using System.Security.Authentication;
using Spectre.Console;

namespace Dotnetsafer.CLI.Helpers
{
    public static class ExceptionHelpers
    {
        public static void ProcessException(Exception exception)
        {
            switch (exception)
            {
                case AuthenticationException:
                    AnsiConsole.WriteLine("");
                    AnsiConsole.MarkupLine("[red]An error has occurred when validating your Api Token or connecting with dotnetsafer.[/]");
                    AnsiConsole.MarkupLine("[darkorange] > Authentication is required to perform this action, please generate an Api token and log in to the console.[/]");
                    AnsiConsole.MarkupLine(AnsiConsole.Profile.Capabilities.Links
                        ? "[green] Please read about CLI authentication at:[/] [link=https://dotnetsafer.com/docs/cli/1.0/Authentication]https://dotnetsafer.com/docs/cli/1.0/Authentication[/]"
                        : "[green] Please read about CLI authentication at:[/] https://dotnetsafer.com/docs/cli/authentication");
                    return;


            }
            AnsiConsole.WriteLine("");
#if DEBUG
            AnsiConsole.WriteException(exception);
#else
            AnsiConsole.MarkupLine("[red]An exception occurred:[/]");
                AnsiConsole.WriteLine("");
                AnsiConsole.MarkupLine($"[darkorange] > {exception.Message}[/]");
                AnsiConsole.MarkupLine(AnsiConsole.Profile.Capabilities.Links
                        ? "[green] Find more information about this error at:[/] [link=https://dotnetsafer.com/docs/cli/1.0/exceptions]https://dotnetsafer.com/docs/cli/1.0/exceptions[/]"
                        : "[green] Find more information about this error at:[/] https://dotnetsafer.com/docs/cli/1.0/exceptions");
#endif
        }
    }
}
