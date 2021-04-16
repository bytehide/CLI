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
using ShieldCLI.Helpers;

namespace ShieldCLI.Commands
{
    public class ShieldCommands


    {
        private ClientManager ClientManager { get; set; }
        private DependenciesResolver DependenciesResolver { get; set; }

        public ShieldCommands(ClientManager clientManager, DependenciesResolver dependenciesResolver)
        {
            ClientManager = clientManager;
            DependenciesResolver = dependenciesResolver;
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
            //ClientManager.Client.Configuration.

            this.OpenBrowser("https://my.dotnetsafer.com/register");
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

        public void ConfigGetFile(string type, string path, string name, bool create)
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
                //if (!File.Exists(fullFilePath))
               //     SaveToFile(path, name);


            }
            else
            {
                ClientManager.Client.Configuration.LoadProjectConfigurationFromFileOrDefault(fullFilePath);
            }



        }

        internal async Task<List<(string, string)>> ResolveDependenciesAsync(string applicationPath)
        {
            var (isValid, requiredDependencies, (module, createdContext)) =
                        await DependenciesResolver.GetAssemblyInfoAsync(applicationPath ?? string.Empty);

            if (!isValid)
                throw new Exception("Invalid .NET Assembly");

            var requiredDep = requiredDependencies.ToList();

            var dependencies = DependenciesResolver.GetUnresolved(module,
                createdContext, requiredDep).ToList();

            var length = dependencies.Count;

            await AnsiConsole.Progress().Columns(
                    new ProgressColumn[]
                    {
                                new TaskDescriptionColumn(),
                                new ProgressBarColumn(),
                                new PercentageColumn(),
                                new RemainingTimeColumn()
                    }
                )
                .StartAsync(async context =>
                {
                    var task1 = context.AddTask("[green]Resolving dependencies[/]");

                    task1.MaxValue = length;

                    foreach (var (assembly, _) in requiredDep.Where(dep => dep.Item2 is null).ToList())
                    {
                        var info = Utils.SplitAssemblyInfo(assembly);

                        await DependenciesResolver.GetUnresolvedWithNuget(
                                module,
                                createdContext, requiredDep, info.name,
                                info.version);

                        task1.Increment(1);
                    }
                });

            while (requiredDep.ToList().Any(dep => string.IsNullOrEmpty(dep.Item2)))
            {
                var unresolved = requiredDep.Where(dep => string.IsNullOrEmpty(dep.Item2)).ToList();

                AnsiConsole.Markup($"The following dependencies [red]({unresolved.Count})[/] are required to process the application:");
                AnsiConsole.WriteLine();

                var table = new Table();

                table.AddColumn("Name").AddColumn("Version");

                table.Border(TableBorder.Rounded);

                var userPath = new List<string>();

                unresolved.ForEach(dep =>
                    table.AddRow(
                        $"[yellow]{Utils.SplitAssemblyInfo(dep.Item1).name}[/]",
                        $"[yellow]{Utils.SplitAssemblyInfo(dep.Item1).version}[/]"));

                AnsiConsole.Render(table);

                unresolved.ForEach(dep => userPath.Add(AnsiConsole.Ask<string>($"Enter the path of the [yellow]{Utils.SplitAssemblyInfo(dep.Item1).name}[/] library:")));

                _ = DependenciesResolver.GetUnresolved(module,
                    createdContext, requiredDep, userPath.Select(Path.GetDirectoryName).ToArray());
            }
            return requiredDep;
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

