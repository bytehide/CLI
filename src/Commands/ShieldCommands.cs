using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        /// <summary>
        ///     Open DotnetSafer web to register a new user
        /// </summary>
        public void AuthRegister()

            => UsefulHelpers.OpenBrowser("https://my.dotnetsafer.com/register");


        /// <summary>
        ///     Log in the current user whit an apiKey.
        /// </summary>
        /// <param name="apiKey">Dotnetsafer Personal Api Token (required to use the CLI)</param>
        /// <see cref="https://dotnetsafer.com/docs/shield-cli/authentication"/>
        public bool AuthLogin(string apiKey)
        {
            if (apiKey is null)
            {
                AnsiConsole.MarkupLine("[blue]Insert your API Key[/]");
                apiKey = Console.ReadLine();
            }

            if (ClientManager.IsValidKey(apiKey))
            {
                ClientManager.UpdateKey(apiKey);
                AnsiConsole.Markup("[lime]Logged in Correctly [/]");
                return true;
            }

            //TODO: Sr-l show help to user
            AnsiConsole.Markup("[red]NOT logged in. Please review the API Key[/]");
            return false;
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
            return AuthLogin(null);
        }

        /// <summary>
        ///     Log out and clear credentials or current user.
        /// </summary>
        public void AuthClearCredentials()
        {
            if (!AnsiConsole.Confirm("[red]This action will DELETE your credentials. Are you sure? [/]")) return;
            ClientManager.ClearClient();
            Console.WriteLine("");
            AnsiConsole.Markup("[red]Credentials deleted. You must to login again to use ShieldCLI [/]");
        }


        /// <summary>
        ///     Gets the configuration file of an application, or creates if <param name="create">create</param> is true.
        /// </summary>
        /// 
        /// <param name="path">Directory path of config file</param>
        /// <param name="name">Name of the application</param>
        /// <param name="create">If <value>true</value> creates the configuration file is not exists</param>
        public ApplicationConfigurationDto GetApplicationConfiguration(string path, string name, bool create)
        {
            var configName = $"Shield.Application.{name}.json";
            var fullFilePath =
                Path.Combine(
                    Path.GetDirectoryName(path) ??
                    throw new InvalidOperationException("The provided directory path doesn't exists."), configName);

            ApplicationConfigurationDto applicationConfig = null;

            if (File.Exists(fullFilePath))
                applicationConfig =
                    ClientManager.Client.Configuration.LoadApplicationConfigurationFromFileOrDefault(fullFilePath);

            else if (create)
                applicationConfig = MakeApplicationConfiguration(path, "balance", name, null);

            return applicationConfig;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public ProjectConfigurationDto GetProjectConfiguration(string path, string name, bool create)
        {
            var fullFilePath = $"{path}/shield.project.${name}.json";

            ProjectConfigurationDto projectConfig = null;

            if (File.Exists(fullFilePath))
                projectConfig =
                    ClientManager.Client.Configuration.LoadProjectConfigurationFromFileOrDefault(fullFilePath);

            if ((!File.Exists(fullFilePath)) && create)
                projectConfig = MakeProjectConfiguration(path, "balance", name, null);

            return projectConfig;

        }

        /// <summary>
        /// Resolves an application required dependencies by his path.
        /// </summary>
        /// <param name="applicationPath">Application path</param>
        /// <returns></returns>
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

                    foreach (var (name, version, _) in from string assembly in requiredDep
                            .Where(dep => dep.Item2 is null).ToList()
                        select Utils.SplitAssemblyInfo(assembly))
                    {
                        await DependenciesResolver.GetUnresolvedWithNuget(
                            module,
                            createdContext, requiredDep, name,
                            version);

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
        /// 
        /// </summary>
        /// <param name="preset"></param>
        /// <returns></returns>
        public string ChooseProtectionPreset(string preset)
        {
            string[] presets = {"maximum", "balance", "custom", "optimized"};

            if (presets.All(pr => pr != preset))
                preset = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("[white]Please choose the preset for the protection of protection[/]")
                        .PageSize(4)
                        .AddChoices(presets));

            return preset;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public string ChooseConfigurationType(string type)
        {

            if (type != "application" && type != "project")
                type = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title(
                            "[white]Protection type must be application or propect.Please choose the type of protection[/]?")
                        .PageSize(3)
                        .AddChoice("project")
                        .AddChoice("application"));
            return type;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string ChooseConfigurationSource()
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

            var availableNames = protections.Where(p => p.Available).Select(p => p.Name).ToList();
            var notAvailableNames = protections.Where(p => !p.Available).Select(p => $"[[PRO]] {p.Name}").ToList();

            var choices = AnsiConsole.Prompt(
                new MultiSelectionPrompt<string>()
                    .Title("Choose custom protections")
                    .PageSize(12)
                    .AddChoices(availableNames)
                    .AddChoices(notAvailableNames)
            );

            var selected = choices.ToArray();

            var available = availableNames.Where(p => selected.Contains(p));
            var notAvailable = notAvailableNames.Where(p => selected.Contains(p)).ToArray();


            if (notAvailable.Length > 0)
            {
                AnsiConsole.MarkupLine(
                    "[darkorange]Following protections selected will not be apply because they are not in your Shield Edition.[/]");
                AnsiConsole.MarkupLine("[darkorange]Please Upgrade your edition if you want to use the protections[/]");
                AnsiConsole.MarkupLine("");
                foreach (var invalid in notAvailable)
                    AnsiConsole.MarkupLine($"[red]{invalid}[/]");

            }

            var selectedIds = protections.Where(p => available.Contains(p.Name)).Select(p => p.Id).ToArray();

            return selectedIds;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="preset"></param>
        /// <param name="name"></param>
        /// <param name="protectionsId"></param>
        /// <returns></returns>
        public ApplicationConfigurationDto MakeApplicationConfiguration(string path, string preset, string name,
            string[] protectionsId)

        {

            var applicationConfig = preset.Equals("custom")
                ? ClientManager.Client.Configuration.MakeApplicationCustomConfiguration(protectionsId)
                : ClientManager.Client.Configuration.MakeApplicationConfiguration(preset.ToPreset());

            applicationConfig.SaveToFile(path, name);

            AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");

            return applicationConfig;

        }

        public ProjectConfigurationDto MakeProjectConfiguration(string path, string preset, string name,
            string[] protectionsId)

        {


            var projectConfig = preset.Equals("custom")
                ? ClientManager.Client.Configuration.MakeProjectCustomConfiguration(protectionsId)
                : ClientManager.Client.Configuration.MakeProjectConfiguration(preset.ToPreset());


            AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");
            projectConfig.SaveToFile(path, name);

            return projectConfig;
        }

        public ProjectDto FindOrCreateProjectByName(string name)
        {
            var project = ClientManager.Client.Project.FindOrCreateExternalProject(name);
            AnsiConsole.Markup("[lime]Project Found [/]");
            return project;
        }

        public ProjectDto FindOrCreateProjectById(string name, string key)
        {
            var project = ClientManager.Client.Project.FindByIdOrCreateExternalProject(name ?? "default", key);
            AnsiConsole.Markup("[lime]Project Found [/]");

            return project;
        }

        public async Task<ProjectDto> FindOrCreateProjectByNameAsync(string name)
            => await ClientManager.Client.Project.FindOrCreateExternalProjectAsync(name);


        public async Task<ProjectDto> FindOrCreateProjectByIdAsync(string name, string key)
        {
            var project =
                await ClientManager.Client.Project.FindByIdOrCreateExternalProjectAsync(name ?? "default", key);
            AnsiConsole.Markup("[lime]Project Found [/]");

            return project;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="keyProject"></param>
        /// <returns></returns>
        public async Task<DirectUploadDto> UploadApplicationAsync(string path, string keyProject)

        {

            var dependencies = await ResolveDependenciesAsync(path);

            var appUpload = await ClientManager.Client.Application.UploadApplicationDirectlyAsync(keyProject,
                path, dependencies.Select(dep => dep.Item2).ToList());

            return appUpload;
        }


        public void PrintProject(string name, string key)
        {
            Console.WriteLine("");
            var table = new Table();

            table.AddColumn("[darkorange]Project Name[/]");
            table.AddColumn("[darkorange]Project Key[/]");
            table.AddRow(name, key);
            AnsiConsole.Render(table);

        }

        public void PrintApplication(string name, string key, string projectKey)
        {
            Console.WriteLine("");
            var table = new Table();

            table.AddColumn("[darkorange]Application Name[/]");
            table.AddColumn("[darkorange]Application Key[/]");
            table.AddColumn("[darkorange]Project Key[/]");

            table.AddRow(name, key, projectKey);
            AnsiConsole.Render(table);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public ApplicationConfigurationDto CreateConfigurationFile(string projectKey, string path)
        {
            ApplicationConfigurationDto configurationDto = null;

            var protection = ChooseConfigurationSource();
            var configName = AnsiConsole.Ask<string>("Enter the config file name");
            AnsiConsole.WriteLine("");

            switch (protection)
            {
                case "Load from a config file":
                {
                    var configPath = AnsiConsole.Ask<string>("Config File Path?");


                    configurationDto = GetApplicationConfiguration(configPath, configName, false);
                    break;
                }
                case "Use a preset":
                {
                    string[] protectionsId = { };
                    var preset = ChooseProtectionPreset("default");
                    if (preset == "custom")
                        protectionsId = ChooseCustomProtections(projectKey);
                    configurationDto = MakeApplicationConfiguration(path, preset, configName, protectionsId);
                    break;
                }
                case "Make a custom":
                {
                    const string preset = "custom";
                    var protectionsId = ChooseCustomProtections(projectKey);
                    configurationDto = MakeApplicationConfiguration(path, preset, configName, protectionsId);
                    break;
                }
            }

            return configurationDto;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="projectKey"></param>
        /// <param name="fileBlob"></param>
        /// <param name="config"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task ProtectApplicationAsync(string projectKey, string fileBlob,
            ApplicationConfigurationDto config, string path)
        {
            var connection = ClientManager.Client.Connector.CreateHubConnection();
            var hub = await ClientManager.Client.Connector.InstanceHubConnectorWithLoggerAsync(connection);
            await hub.StartAsync();

            ProtectionResult result = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Balloon2).StartAsync("We are protecting your application...", async ctx =>
                {


                    result = await ClientManager.Client.Tasks.ProtectSingleFileAsync(projectKey, fileBlob, connection,
                        config);



                    //hub.OnLog(connection.OnLogger, (string s, string s1, string s2) =>
                    //{
                    //    AnsiConsole.WriteLine(s2);

                    //});
                });

            result.OnSuccess(hub, async (application) =>
                {
                    AnsiConsole.MarkupLine(
                        $"[lime]The application has been PROTECTED SUCESSFULLY with {application.Preset} protection. [/]");
                    AnsiConsole.MarkupLine("");
                    var downloaded =
                        await ClientManager.Client.Application.DownloadApplicationAsArrayAsync(application);
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