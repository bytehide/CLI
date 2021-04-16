using Microsoft.Extensions.Configuration;
using Shield.Client;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Repos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Shield.Client.Extensions;

namespace ShieldCLI.Commands
{
    public class ShieldCommands


    {
        private ClientManager ClientManager { get; set; }

        public ShieldCommands(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }

        //public void saludoShield()
        //{

        //    AnsiConsole.Markup("[fuchsia]Este es el método en ShiedCommands[/]");

        //}


        /// <summary>
        /// Open DotnetSafer web to register a new user
        /// </summary>
        public void AuthRegister()
        {
            ClientManager.Client.Configuration.

            OpenBrowser("https://my.dotnetsafer.com/register");
            return;

        }

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

        /// <summary>
        /// Log in the current user whit an apiKey.
        /// </summary>
        /// <param name="apiKey">APIKEY of Dotnetsafer to use the CLI</param>
        public void AuthDoLogin(string apiKey)
        {
            if (apiKey == null)
            {
                AnsiConsole.MarkupLine("[blue]Insert your API Key[/]");
                apiKey = Console.ReadLine();
            }


            if (ClientManager.IsValidKey(apiKey))
            {

                ClientManager.UpdateKey(apiKey);
                AnsiConsole.Markup("[lime]Logged in Correctly [/]");

            }
            else
            {

                AnsiConsole.Markup("[red]NOT logged in. Please review the API Key[/]");
            }


        }

        /// <summary>
        /// Checks if user is logged in.
        /// </summary>
        public bool AuthHasCredentials()
        {

            if (!ClientManager.HasValidClient())
            {
                AnsiConsole.MarkupLine("[red]You are NOT logged in. \nYou must be logged in to use Dotnetsafer.[/]");


                Console.WriteLine("");
                if (!AnsiConsole.Confirm("[blue]Do you want to logged in now? [/]"))
                {
                    return false;
                }

                Console.WriteLine("");
                AuthDoLogin(null);
            }


            return true;
        }

        /// <summary>
        /// Log out and clear credentials or current user. 
        /// </summary>
        public void AuthClearCredentials()
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


        /// <summary>
        /// Find the config files in a paht
        /// </summary>
        /// <param name="type"></param>
        /// <param name="path"></param>
        /// <param name="name"></param>

        public ConfigGetFile(string type, string path, string name, bool create)
        {


            if (type != "application" && type != "project")
            {
                type = AnsiConsole.Prompt(
                         new SelectionPrompt<string>()
                         .Title("[white]Please choose the type of protection[/]?")
                         .PageSize(3)
                         .AddChoice("project")
                         .AddChoice("application"));
            }

            string fullFilePath = $"{path}/shield.{type}.{name}.json";


            if (type == "application")
            {
                ClientManager.Client.Configuration.LoadApplicationConfigurationFromFileOrDefault(fullFilePath);
                if (!File.Exists(fullFilePath))
               //     SaveToFile(path, name);


            }
            else
            {
                ClientManager.Client.Configuration.LoadProjectConfigurationFromFileOrDefault(fullFilePath);
            }



        }




        /// <summary>
        /// Make a config file 
        /// </summary>
        /// <param name="type">type of the config file : application or project</param>
        /// <param name="path">Path were config file is created</param>
        /// <param name="preset">Shield preset to de protection of application or project</param>
        /// <param name="name">Name of the file</param>
        public void ConfigMakeFile(string type, string path, string preset, string name)

        {
            string[] presets = { "maximum", "balance", "custom", "optimized" };
            string[] protectionsId = { "protrection1", "protection2" };

            if (type != "application" && type != "project")
            {
                type = AnsiConsole.Prompt(
                         new SelectionPrompt<string>()
                         .Title("[white]Please choose the type of protection[/]?")
                         .PageSize(3)
                         .AddChoice("project")
                         .AddChoice("application"));
            }


            if (!Array.Exists(presets, element => element == preset))
            {
                preset = AnsiConsole.Prompt(
                           new SelectionPrompt<string>()
                            .Title("[white]Please choose the preset for the protection of protection[/]?")
                             .PageSize(4)
                            .AddChoice("maximum")
                            .AddChoice("balance")
                        .AddChoice("optimized")
                        .AddChoice("custom"));

            }

            ProjectConfigurationDto projectConfig;
            ApplicationConfigurationDto applicationConfig;

            if (type == "application")
            {
                applicationConfig = preset.Equals("custom") ? ClientManager.Client.Configuration.MakeApplicationCustomConfiguration(protectionsId) :
                ClientManager.Client.Configuration.MakeApplicationConfiguration(ShieldConfigurationPresets.ToPreset(preset));

                applicationConfig.SaveToFile(path, name);
            }
            else
            {
                projectConfig = preset.Equals("custom") ? ClientManager.Client.Configuration.MakeProjectCustomConfiguration(protectionsId) :
                ClientManager.Client.Configuration.MakeProjectConfiguration(ShieldConfigurationPresets.ToPreset(preset));

                projectConfig.SaveToFile(path, name);

            }


            AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");




        }
    }
}

