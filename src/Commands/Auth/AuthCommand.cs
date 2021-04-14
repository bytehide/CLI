using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using MatthiWare.CommandLine.Abstractions.Command;
using ShieldCLI.Models;
using ShieldCLI.Models.Auth;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands.Auth
{
    public class AuthCommand : Command<GlobalOptions, AuthOptions>
    {

        private ClientManager ClientManager { get; set; }

        public AuthCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }

        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("auth").Description("Log or register a user in Shield.");
        }

        public override void OnExecute(GlobalOptions options, AuthOptions auth)
        {
            if (auth.Register)
            {
                OpenBrowser("https://my.dotnetsafer.com/register");
                return;
            }

            if (auth.Clear)
            {
                if (!AnsiConsole.Confirm("[red]This action will DELETE your credentials. Are you sure? [/]"))
                {
                    return;
                }
                ClientManager.ClearClient();
                Console.WriteLine("");
                AnsiConsole.Markup("[red]Credentials deleted. You must to loggin again to use ShieldCLI [/]");
                return;
            }

            if (auth.Check)
            {
                if (ClientManager.HasValidClient())
                {
                    AnsiConsole.Markup("[lime]You are logged in. [/]");
                }
                else
                {
                    AnsiConsole.Markup("[red]You are NOT logged in. [/]");
                    Console.WriteLine("");
                    if (!AnsiConsole.Confirm("[blue]Do you want to logged in now? [/]"))
                    {
                        return;
                    }
                    Console.WriteLine("");


  
                    AnsiConsole.Markup("[blue]Insert your API Key[/]");


                    var apiKey = Console.ReadLine();
                    AnsiConsole.Markup(login(apiKey));

                    //if (login(apiKey))
                    //{
                    //    AnsiConsole.Markup("[lime]Logged in Correctly[/]");

                    //}
                    //else
                    //{
                    //    AnsiConsole.Markup("[red]NOT logged in. Please review the API Key[/]");
                    //}


                }

            }
            if (auth.Login != null)
            {
                AnsiConsole.Markup(login(auth.Login));
                //if (login(auth.Login))
                //{
                //    AnsiConsole.Markup("[lime]Logged in Correctly[/]");
                //}
                //else
                //{
                //    AnsiConsole.Markup("[red]NOT logged in. Please review the API Key[/]");

                //}
            }


        }

        public string login(string apiKey)
            {
            string message = "";
                if (ClientManager.IsValidKey(apiKey))
                {
                    ClientManager.UpdateKey(apiKey);

                    message = "[lime]Logged in Correctly [/]";
            }
            else
                {

                message = "[red]NOT logged in. Please review the API Key[/]";
                }
            return message;
            }

            //if (!ClientManager.HasValidClient())
            //            ClientManager.UpdateKey("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjA5ZmUzOGY3LWM0MGYtNGFjNy04YTJmLTc4MDQxYWFlZWFmYyIsInVuaXF1ZV9uYW1lIjoiOTMwOGVhZGMtMTc5NC00NzE3LWFiY2YtZTM5ZjVhM2FmMDQ3IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6IjU4YWI0NjU0LWJiZTgtNDM2Mi1iZDM0LTk4YTZlZWFjMzJiNSIsImV4cCI6MTYxODQwNjEyMn0._cTWvkNmrF75GJkP_yiDoek5meI4IHLIbZLnwl9ZmlE");

            //var ahoraesvalid = ClientManager.HasValidClient();


            // esto es para abrir web en navegador
            public void OpenBrowser(string url)
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);  // Works ok on linux
                }
                else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    Process.Start("open", url); // Not tested
                }

            }


        
    } 
}
