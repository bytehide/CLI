using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Shield.Client.Extensions;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Helpers;
using ShieldCLI.Repos;
using Spectre.Console;

namespace ShieldCLI.Commands
{
    public class ShieldCommands


    {
        public ShieldCommands(ClientManager clientManager, DependenciesResolver dependenciesResolver)
        {
            ClientManager = clientManager;
            DependenciesResolver = dependenciesResolver;
        }

        private ClientManager ClientManager { get; }
        private DependenciesResolver DependenciesResolver { get; }

        //public void saludoShield()
        //{

        //    AnsiConsole.Markup("[fuchsia]Este es el método en ShiedCommands[/]");

        //}


        /// <summary>
        ///     Open DotnetSafer web to register a new user
        /// </summary>
        public void AuthRegister()
        {
            //ClientManager.Client.Configuration.

            OpenBrowser("https://my.dotnetsafer.com/register");
        }

        public void OpenBrowser(string url)
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                Process.Start(new ProcessStartInfo(url) { UseShellExecute = true }); // Works ok on windows
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                Process.Start("xdg-open", url); // Works ok on linux
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) Process.Start("open", url); // Not tested
        }





        /// <summary>
        ///     Log in the current user whit an apiKey.
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
        ///     Checks if user is logged in.
        /// </summary>
        public bool AuthHasCredentials()
        {
            if (ClientManager.HasValidClient()) return true;

            AnsiConsole.MarkupLine("[red]You are NOT logged in. \nYou must be logged in to use Dotnetsafer.[/]");
            Console.WriteLine("");

            if (!AnsiConsole.Confirm("[blue]Do you want to logged in now? [/]"))
                return false;

            Console.WriteLine("");
            AuthDoLogin(null);

            return true;
        }

        /// <summary>
        ///     Log out and clear credentials or current user.
        /// </summary>
        public void AuthClearCredentials()
        {
            if (!AnsiConsole.Confirm("[red]This action will DELETE your credentials. Are you sure? [/]")) return;
            ClientManager.ClearClient();
            Console.WriteLine("");
            AnsiConsole.Markup("[red]Credentials deleted. You must to loggin again to use ShieldCLI [/]");
        }


        /// <summary>
        ///     Find the config files in a paht
        /// </summary>
        /// 
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="create"></param>
        public ApplicationConfigurationDto ConfigApplicationGetFile(string path, string name, bool create)
        {
            var configName = $"Shield.Application.{name}.json";
            var fullFilePath = Path.Combine(Path.GetDirectoryName(path), configName);

            ApplicationConfigurationDto applicationConfig = null;

            if (File.Exists(fullFilePath))
                applicationConfig = ClientManager.Client.Configuration.LoadApplicationConfigurationFromFileOrDefault(fullFilePath);

            if ((!File.Exists(fullFilePath)) && create)
                applicationConfig = ConfigApplicationMakeFile(path, "balance", name, null);

            return applicationConfig;

        }

        public ProjectConfigurationDto ConfigProjectGetFile(string path, string name, bool create)
        {
            var fullFilePath = $"{path}/shield.project.${name}.json";

            ProjectConfigurationDto projectConfig = null;

            if (File.Exists(fullFilePath))
                projectConfig = ClientManager.Client.Configuration.LoadProjectConfigurationFromFileOrDefault(fullFilePath);

            if ((!File.Exists(fullFilePath)) && create)
                projectConfig = ConfigProjectMakeFile(path, "balance", name, null);

            return projectConfig;

        }
        internal async Task<List<(string, string)>> ResolveDependenciesAsync(string applicationPath)
        {
            var (isValid, requiredDependencies, (module, createdContext)) =
                await DependenciesResolver.GetAssemblyInfoAsync(applicationPath ?? string.Empty);

            if (!isValid)
                throw new Exception("Invalid .NET Assembly");

            var requiredDep = requiredDependencies.ToList();

            AnsiConsole.Markup("[green]Resolving dependencies locally...[/]");
            AnsiConsole.WriteLine();

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
                    var resolverTask = context.AddTask("[green]Resolving dependencies with nuget...[/]");

                    resolverTask.MaxValue = length;

                    foreach (var (assembly, _) in requiredDep.Where(dep => dep.Item2 is null).ToList())
                    {
                        var info = Utils.SplitAssemblyInfo(assembly);

                        await DependenciesResolver.GetUnresolvedWithNuget(
                            module,
                            createdContext, requiredDep, info.name,
                            info.version);

                        resolverTask.Increment(1);
                    }
                });

            while (requiredDep.ToList().Any(dep => string.IsNullOrEmpty(dep.Item2)))
            {
                var unresolved = requiredDep.Where(dep => string.IsNullOrEmpty(dep.Item2)).ToList();

                AnsiConsole.Markup(
                    $"The following dependencies [red]({unresolved.Count})[/] are required to process the application:");
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

                unresolved.ForEach(dep =>
                    userPath.Add(AnsiConsole.Ask<string>(
                        $"Enter the path of the [yellow]{Utils.SplitAssemblyInfo(dep.Item1).name}[/] library:")));

                _ = DependenciesResolver.GetUnresolved(module,
                    createdContext, requiredDep, userPath.Select(Path.GetDirectoryName).ToArray());
            }

            return requiredDep;
        }

        /// <summary>
        ///     Make a config file
        /// </summary>
        /// <param name="type">type of the config file : application or project</param>
        /// <param name="path">Path were config file is created</param>
        /// <param name="preset">Shield preset to de protection of application or project</param>
        /// <param name="name">Name of the file</param>

        public string ChoosePreset(string preset)
        {
            string[] presets = { "maximum", "balance", "custom", "optimized" };

            if (presets.All(pr => pr != preset))
                preset = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[white]Please choose the preset for the protection of protection[/]?")
                        .PageSize(4)
                        .AddChoices(presets));

            return preset;

        }

        public string ChooseType(string type)
        {

            if (type != "application" && type != "project")
                type = AnsiConsole.Prompt(
                     new SelectionPrompt<string>()
                         .Title("[white]Protection type must be application or propect.Please choose the type of protection[/]?")
                         .PageSize(3)
                         .AddChoice("project")
                         .AddChoice("application"));
            return type;

        }

        public string ChooseProtections()
        {
            AnsiConsole.WriteLine();
            var value = AnsiConsole.Prompt(
              new SelectionPrompt<string>()

                .Title("[dodgerblue3]Choose the source of protection to use[/]")
                .PageSize(3)
                .AddChoice("Load from a config file")
                .AddChoice("Use a preset")
                .AddChoice("Make a custom")

              );

            return value;
        }
        public string[] ChooseCustomProtections(string projectKey)
        {
            var protections = ClientManager.Client.Protections.GetProtections(projectKey);

            var allAviableNames = protections.Where(p => p.Available).Select(p => p.Name).ToList();
            var allNotAvailableNames = protections.Where(p => !p.Available).Select(p => $"[[PRO]] {p.Name}").ToList();

            var allNames = protections.Select(p => p.Name).ToList();
            var choices = AnsiConsole.Prompt(
                     new MultiSelectionPrompt<string>()
             .Title("Choose custom protections?")
             .PageSize(12)
             .AddChoices(allAviableNames)
             .AddChoices(allNotAvailableNames)
             );
            var elected = choices.ToArray();

            var electedAndAviable = allAviableNames.Where(p => elected.Contains(p));
            var electedAndNotAviable = allNotAvailableNames.Where(p => elected.Contains(p)).ToArray();


            if (electedAndNotAviable.Length > 0)
            {
                AnsiConsole.MarkupLine("[darkorange]Following protections selected will not be aply because they are not in your Shield Edition.[/]");
                AnsiConsole.MarkupLine("[darkorange]Please Upgrade your edition if you want to use the protections[/]");
                AnsiConsole.MarkupLine("");
                foreach (string elegido in electedAndNotAviable)

                {
                    AnsiConsole.MarkupLine($"[red]{elegido}[/]");
                }
            }
            Thread.Sleep(1500);

            var idsElectedAndAviable = protections.Where(p => electedAndAviable.Contains(p.Name)).Select(p => p.Id).ToArray();

            return idsElectedAndAviable;


        }

        public ApplicationConfigurationDto ConfigApplicationMakeFile(string path, string preset, string name, string[] protectionsId)

        {

            var applicationConfig = preset.Equals("custom")
                ? ClientManager.Client.Configuration.MakeApplicationCustomConfiguration(protectionsId)
                : ClientManager.Client.Configuration.MakeApplicationConfiguration(preset.ToPreset());

            applicationConfig.SaveToFile(path, name);

            AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");

            return applicationConfig;

        }

        public ProjectConfigurationDto ConfigProjectMakeFile(string path, string preset, string name, string[] protectionsId)

        {


            var projectConfig = preset.Equals("custom") ? ClientManager.Client.Configuration.MakeProjectCustomConfiguration(protectionsId)
                : ClientManager.Client.Configuration.MakeProjectConfiguration(preset.ToPreset());


            AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");
            projectConfig.SaveToFile(path, name);

            return projectConfig;
        }

        public ProjectDto ProjectFindOrCreateByName(string name)
        {
            ProjectDto project = ClientManager.Client.Project.FindOrCreateExternalProject(name);
            AnsiConsole.Markup("[lime]Project Found [/]");
            return project;
        }

        public ProjectDto ProjectFindOrCreateById(string name, string key)
        {
            var project = ClientManager.Client.Project.FindByIdOrCreateExternalProject(name ?? "default", key);
            AnsiConsole.Markup("[lime]Project Found [/]");

            return project;
        }

        public async Task<ProjectDto> ProjectFindOrCreateByNameAsync(string name)
        {
            ProjectDto project = await ClientManager.Client.Project.FindOrCreateExternalProjectAsync(name);
            //AnsiConsole.Markup("[lime]Project Found [/]");

            return project;
        }
        public async Task<ProjectDto> ProjectFindOrCreateByIdAsync(string name, string key)
        {
            var project = await ClientManager.Client.Project.FindByIdOrCreateExternalProjectAsync(name ?? "default", key);
            AnsiConsole.Markup("[lime]Project Found [/]");

            return project;
        }

        public async Task<DirectUploadDto> UploadApplicationAsync(string path, string keyProject)

        {

            var dependencies = await ResolveDependenciesAsync(path);

            var appUpload = await ClientManager.Client.Application.UploadApplicationDirectlyAsync(keyProject,
                 path, dependencies.Select(dep => dep.Item2).ToList());

            return appUpload;
        }


        public void ProjectTable(string name, string key)
        {
            Console.WriteLine("");
            var table = new Table();

            // Add some columns
            table.AddColumn("[darkorange]Project Name[/]");
            table.AddColumn("[darkorange]Project Key[/]");
            // Add some rows
            table.AddRow(name, key);
            // Render the table to the console
            AnsiConsole.Render(table);

        }

        public void ApplicationtTable(string name, string key, string projectKey)
        {
            Console.WriteLine("");
            var table = new Table();

            // Add some columns
            table.AddColumn("[darkorange]Application Name[/]");
            table.AddColumn("[darkorange]Application Key[/]");
            table.AddColumn("[darkorange]Project Key[/]");

            // Add some rows
            table.AddRow(name, key, projectKey);
            // Render the table to the console
            AnsiConsole.Render(table);

        }
        public ApplicationConfigurationDto CreateConfigFile(string projectKey, string path)
        {
            ApplicationConfigurationDto configurationDto = null;

            string protection = ChooseProtections();
            string configname = AnsiConsole.Ask<string>("Enter the config file name");
            AnsiConsole.WriteLine("");

            if (protection == "Load from a config file")

            {

                var configPath = AnsiConsole.Ask<string>("Config File Path?");


                configurationDto = ConfigApplicationGetFile(configPath, configname, false);

            }

            if (protection == "Use a preset")

            {
                string[] protectionsId = { };
                var preset = ChoosePreset("default");
                if (preset == "custom")
                    protectionsId = ChooseCustomProtections(projectKey);
                configurationDto = ConfigApplicationMakeFile(path, preset, configname, protectionsId);

            }
            if (protection == "Make a custom")
            {
                var preset = "custom";
                var protectionsId = ChooseCustomProtections(projectKey);
                configurationDto = ConfigApplicationMakeFile(path, preset, configname, protectionsId);
            }

            return configurationDto;
        }



        public async Task ProtectApplicationAsync(string projectKey, string fileBlob, ApplicationConfigurationDto config, string path)
        {
            var connection = ClientManager.Client.Connector.CreateHubConnection();
            var hub = await ClientManager.Client.Connector.InstanceHubConnectorWithLoggerAsync(connection);
            await hub.StartAsync();

            ProtectionResult result = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Balloon2).StartAsync("We are protecting your application...", async ctx =>
                         {


                             result = await ClientManager.Client.Tasks.ProtectSingleFileAsync(projectKey, fileBlob, connection, config);



                             //hub.OnLog(connection.OnLogger, (string s, string s1, string s2) =>
                             //{
                             //    AnsiConsole.WriteLine(s2);

                             //});
                         });

            result.OnSuccess(hub, async (application) =>
                {
                    AnsiConsole.MarkupLine($"[lime]The application has been PROTECTED SUCESSFULLY with {application.Preset} protection. [/]");
                    AnsiConsole.MarkupLine("");
                    var downloaded = await ClientManager.Client.Application.DownloadApplicationAsArrayAsync(application);
                    downloaded.SaveOn(path, true);
                    var savedDir = Path.GetDirectoryName(path);
                    AnsiConsole.MarkupLine($"[lime]Application SAVED SUCESSFULLY in [/][darkorange]{savedDir}[/]");
                }
             );

            var semaphore = new Semaphore(0, 1);

            result.OnError(hub, AnsiConsole.Write);

            result.OnClose(hub, (s) =>
            {
                semaphore.Release();
                AnsiConsole.Markup($"[lime]{s} [/]");
            });

            semaphore.WaitOne();
        }



    }
}