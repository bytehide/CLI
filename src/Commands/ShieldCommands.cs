using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Threading.Tasks;
using Dotnetsafer.CLI.Helpers;
using Dotnetsafer.CLI.Repos;
using Microsoft.Extensions.Logging;
using Shield.Client.Extensions;
using Shield.Client.Models;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using Spectre.Console;

namespace Dotnetsafer.CLI.Commands
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
        /// <see cref="https://dotnetsafer.com/docs/cli/1.0/authentication"/>
        public bool AuthLogin(string apiKey)
        {
            apiKey ??= AnsiConsole.Ask<string>("[blue]Insert your API Key[/]");

            if (ClientManager.IsValidKey(apiKey))
            {
                ClientManager.UpdateKey(apiKey);
                AnsiConsole.MarkupLine(
                    "[lime]Logged in correctly. Your session has been saved, to delete you credentials use [dim]clear[/][/]");
                AnsiConsole.WriteLine("");
                return true;
            }

            //TODO: Sr-l show help to user
            AnsiConsole.MarkupLine("[red]NOT logged in. Please review the API Key.[/]");
            AnsiConsole.MarkupLine(AnsiConsole.Profile.Capabilities.Links
                ? "[green] Read about CLI authentication at:[/] [link=https://dotnetsafer.com/docs/cli/1.0/authentication]https://dotnetsafer.com/docs/cli/1.0/authentication[/]"
                : "[green] Read about CLI authentication at:[/] https://dotnetsafer.com/docs/cli/1.0/authentication");
            AnsiConsole.WriteLine("");
            return false;
        }

        /// <summary>
        ///     Checks if user is logged in.
        /// </summary>
        public bool AuthHasCredentials(bool throwException = true)
        {
            const string exMessage = "User is not logged into dotnetsafer.";

            if (ClientManager.HasValidClient()) return true;

            AnsiConsole.MarkupLine("[red]You are NOT logged in. \nYou must be logged in to use Shield CLI.[/]");
            AnsiConsole.WriteLine("");

            if (!AnsiConsole.Confirm("[blue]Do you want to logged in now? [/]"))
                return !throwException ? false : throw new AuthenticationException(exMessage);

            AnsiConsole.WriteLine("");
            var login = AuthLogin(null);
            return login ? true : (throwException ? throw new AuthenticationException(exMessage) : false);
        }

        /// <summary>
        ///     Log out and clear credentials or current user.
        /// </summary>
        public void AuthClearCredentials()
        {
            if (!AnsiConsole.Confirm("[red]This action will DELETE your credentials. Are you sure? [/]")) return;
            ClientManager.ClearClient();
            Console.WriteLine("");
            AnsiConsole.MarkupLine("[red]Credentials deleted. You must to login again to use ShieldCLI [/]");
        }



        public string CreateFullPath(string dirPath, string name)
        {


            var separator = Path.DirectorySeparatorChar;

            var configPath = Path.EndsInDirectorySeparator(dirPath) ? dirPath : $"{dirPath}{separator}";
            var fullFilePath =
                Path.Combine(
                    Path.GetDirectoryName(configPath) ??
                    throw new InvalidOperationException("The provided directory path doesn't exists."), name);


            return fullFilePath;
        }

        public string GetFilesConfig(string path)
        {
            var applicationsFiles = Directory.GetFiles(path, "shield.application.*.json").ToArray();
            var projectFiles = Directory.GetFiles(path, "shield.project.*.json").ToArray();

            var allConfigFiles = applicationsFiles.Concat(projectFiles);

            if (allConfigFiles.Count() == 0)
            {
                AnsiConsole.MarkupLine($"[darkorange]There is no config files in this path[/]");
                return "";
            }
            string fullConfigName = allConfigFiles.First();

            if (allConfigFiles.Count() != 1)
            {
                var file = AnsiConsole.Prompt(
                       new MultiSelectionPrompt<string>()
                      .Title("Choose the Config File")
                      .PageSize(12)
                      .AddChoices(allConfigFiles));

                fullConfigName = file.First();
            }



            return fullConfigName;
        }

        public void PrintConfigFiles(string path, string preset, List<string> protections)
        {
            var name = Path.GetFileName(path);

            AnsiConsole.MarkupLine("[lime] Config File has the follow info: [/] ");

            var root = new Tree(name).Style("lime").Guide(TreeGuide.DoubleLine);


            var presetbranch = root.AddNode("[darkorange]Preset[/]");

            presetbranch.AddNode(preset);

            if (preset == "custom")
            {
                var protectionsBranch = root.AddNode("[darkorange]Protections[/]");

                foreach (string protection in protections)
                {
                    protectionsBranch.AddNode(protection);

                }
            }
            AnsiConsole.Render(root);

        }

        /// <summary>
        ///     Gets the configuration file of an application, or creates if <param name="create">create</param> is true.
        /// </summary>
        /// 
        /// <param name="path">Directory path of config file</param>
        /// <param name="name">Name of the application</param>
        /// <param name="create">If <value>true</value> creates the configuration file is not exists</param>
        public ApplicationConfigurationDto GetApplicationConfiguration(string fullFilePath, bool create)
        {



            ApplicationConfigurationDto applicationConfig = null;

            if (File.Exists(fullFilePath))
                applicationConfig =
                    ClientManager.Client.Configuration.LoadApplicationConfigurationFromFileOrDefault(fullFilePath);

            //else if (create)
            //    applicationConfig = MakeApplicationConfiguration(path, "balance", name, null);


            return applicationConfig;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="path"></param>
        /// <param name="name"></param>
        /// <param name="create"></param>
        /// <returns></returns>
        public ProjectConfigurationDto GetProjectConfiguration(string fullFilePath, bool create)
        {


            ProjectConfigurationDto projectConfig = null;

            if (File.Exists(fullFilePath))
            {
                projectConfig =
                    ClientManager.Client.Configuration.LoadProjectConfigurationFromFileOrDefault(fullFilePath);
            }
            //else if (create)
            //{


            //    projectConfig = MakeProjectConfiguration(path, "balance", name, null);
            //}

            return projectConfig;
            ///TODO: @Sr-l Read file and show info. 

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
                throw new Exception(
                    "Invalid .NET Assembly. The application is not a .NET module, remember that if it is .NET Core you must protect the compiled .dll (NOT .exe).");

            var requiredDep = requiredDependencies.ToList();

            List<string> dependencies = null;


            //AnsiConsole.Markup("[green]Resolving dependencies locally...[/]");

            AnsiConsole.Status()
                .Start("[green]Resolving dependencies locally...[/]", ctx =>
                {
                    ctx.Spinner(Spinner.Known.BoxBounce);
                    ctx.SpinnerStyle(Style.Parse("green"));

                    dependencies = DependenciesResolver.GetUnresolved(module,
                        createdContext, requiredDep).ToList();
                });

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

                    resolverTask.StopTask();
                });

            while (requiredDep.ToList().Any(dep => string.IsNullOrEmpty(dep.Item2)))
            {
                var unresolved = requiredDep.Where(dep => string.IsNullOrEmpty(dep.Item2)).ToList();

                AnsiConsole.MarkupLine(
                    $"The following dependencies [red]({unresolved.Count})[/] are required to process the application:");
                AnsiConsole.WriteLine();

                var table = new Table();

                table.AddColumn("Name").AddColumn("Version");

                table.Border(TableBorder.Rounded);

                var userPath = new List<string>();

                unresolved.ForEach(dep =>
                    table.AddRow(
                        $"[darkorange]{Utils.SplitAssemblyInfo(dep.Item1).name}[/]",
                        $"[darkorange]{Utils.SplitAssemblyInfo(dep.Item1).version}[/]"));

                AnsiConsole.Render(table);

                AnsiConsole.WriteLine("");

                unresolved.ForEach(dep =>
                    userPath.Add(AnsiConsole.Ask<string>(
                        $"[darkorange]Enter the path of the [red]{Utils.SplitAssemblyInfo(dep.Item1).name}[/] library:[/]")));

                _ = DependenciesResolver.GetUnresolved(module,
                    createdContext, requiredDep, userPath.ToArray());
            }

            AnsiConsole.MarkupLine("[lime]The dependencies have been resolved.[/]");

            return requiredDep;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="preset"></param>
        /// <returns></returns>
        public string ChooseProtectionPreset(string preset)
        {
            string[] presets = { "maximum", "balance", "custom", "optimized" };

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

                    .Title("[darkorange]Choose the source of protection to use[/]")
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

            if (File.Exists(Path.Combine(path, name)))
            {
                File.Delete(Path.Combine(path, name));
            }


            applicationConfig.SaveToFile(path, name);

            AnsiConsole.MarkupLine("[lime]Application configuration file created sucessfully.[/]");

            return applicationConfig;

        }

        public ProjectConfigurationDto MakeProjectConfiguration(string path, string preset, string name,
            string[] protectionsId)

        {


            var projectConfig = preset.Equals("custom")
                ? ClientManager.Client.Configuration.MakeProjectCustomConfiguration(protectionsId)
                : ClientManager.Client.Configuration.MakeProjectConfiguration(preset.ToPreset());

            if (File.Exists(Path.Combine(path, name)))
            {
                File.Delete(Path.Combine(path, name));
            }

            projectConfig.SaveToFile(path, name);
            AnsiConsole.MarkupLine("[lime]Project configuration file created sucessfully.[/]");

            return projectConfig;
        }

        public ProjectDto FindOrCreateProjectByName(string name)
        {
            var project = ClientManager.Client.Project.FindOrCreateExternalProject(name);
            AnsiConsole.MarkupLine("[lime]Project Found [/]");
            return project;
        }

        public ProjectDto FindOrCreateProjectById(string name, string key)
        {
            var project = ClientManager.Client.Project.FindByIdOrCreateExternalProject(name ?? "default", key);
            AnsiConsole.MarkupLine("[lime]Project Found [/]");

            return project;
        }

        public async Task<ProjectDto> FindOrCreateProjectByNameAsync(string name)
        {
            ProjectDto result = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.BoxBounce2)
                .SpinnerStyle(Style.Parse("green bold"))
                .StartAsync("Loading project...",
                    async ctx =>
                    {
                        result = await ClientManager.Client.Project.FindOrCreateExternalProjectAsync(name);
                    });

            return result;
        }


        public async Task<ProjectDto> FindOrCreateProjectByIdAsync(string name, string key)
        {
            ProjectDto result = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.BoxBounce2)
                .SpinnerStyle(Style.Parse("green bold"))
                .StartAsync("Looking for project...", async ctx =>
                {
                    result = await ClientManager.Client.Project.FindByIdOrCreateExternalProjectAsync(name ?? "default",
                        key);
                    AnsiConsole.Markup("[lime]Project found.[/]");
                });

            return result;
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

            DirectUploadDto result = null;

            await AnsiConsole.Status()
                .Spinner(Spinner.Known.BoxBounce2)
                .SpinnerStyle(Style.Parse("green bold"))
                .StartAsync("Processing application...", async ctx =>
                {
                    result = await ClientManager.Client.Application.UploadApplicationDirectlyAsync(keyProject,
                        path, dependencies.Select(dep => dep.Item2).ToList());
                });

            Console.WriteLine("");
            AnsiConsole.MarkupLine("[lime]Application Uploaded Succesfully[/]");
            return result;

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
        /// <param name="applicationName"></param>
        /// <returns></returns>
        public ApplicationConfigurationDto CreateConfigurationFile(string projectKey, string path,
            string applicationName = null)
        {
            ApplicationConfigurationDto configurationDto = null;

            var protection = ChooseConfigurationSource();

            var text = new TextPrompt<string>("[lime]Enter the config file name[/]");
            if (!string.IsNullOrEmpty(applicationName))
                text.DefaultValue(applicationName);

            var configName =
                AnsiConsole.Prompt(text);

            AnsiConsole.WriteLine("");

            switch (protection)
            {
                case "Load from a config file":
                    {
                        var configPath = AnsiConsole.Ask<string>("[lime]Provide the configuration file path:[/]");

                        configurationDto = GetApplicationConfiguration(configPath, false);

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
        /// <param name="appName"></param>
        /// <param name="fileBlob"></param>
        /// <param name="config"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public async Task ProtectApplicationAsync(string projectKey, string fileBlob,
            ApplicationConfigurationDto config, string path)
        {
            await AnsiConsole.Status()
                .Spinner(Spinner.Known.Dots2).StartAsync("The application is being protected...", async ctx =>
                {
                    var connection = ClientManager.Client.Connector.CreateHubConnection();
                    var hub = await ClientManager.Client.Connector.InstanceHubConnectorWithLoggerAsync(connection);
                    await hub.StartAsync();

                    var result = await ClientManager.Client.Tasks.ProtectSingleFileAsync(projectKey, fileBlob, connection,
                        config);

                    hub.OnLog(connection.OnLogger, (string date, string message, string level) =>
                    {
                        _ = Enum.TryParse<LogLevel>(level, out var logLevel);

                        const LogLevel minimumLevel = LogLevel.Information;

                        var color = logLevel switch
                        {
                            LogLevel.Trace => Color.Cyan3.ToString(),
                            LogLevel.Debug => Color.DarkViolet.ToString(),
                            LogLevel.Information => Color.DodgerBlue3.ToString(),
                            LogLevel.Warning => Color.DarkOrange.ToString(),
                            LogLevel.Error => Color.DarkRed.ToString(),
                            LogLevel.Critical => Color.Red.ToString(),
                            LogLevel.None => Color.Black.ToString(),
                            _ => throw new ArgumentOutOfRangeException()
                        };

                        if (logLevel < minimumLevel)
                            return;

                        AnsiConsole.MarkupLine("[" + color + "] > {0}[/]", message.EscapeMarkup());
                    });

                    result.OnSuccess(hub, async (application) =>
                        {
                            AnsiConsole.MarkupLine("");
                            AnsiConsole.MarkupLine(
                                $"[lime] > The application has been protected successfully with {application.Preset} protection.[/]");
                            AnsiConsole.MarkupLine("");
                            var downloaded =
                               await ClientManager.Client.Application.DownloadApplicationAsArrayAsync(application);


                            downloaded.SaveOn(path, true);

                            AnsiConsole.MarkupLine(
                                $"[lime]Application saved successfully in [/][darkorange]{Path.GetDirectoryName(path)}[/]");
                        }
                    );

                    var semaphore = new Semaphore(0, 1);

                    result.OnError(hub, (error) =>
                    {
                        AnsiConsole.MarkupLine("");
                        AnsiConsole.MarkupLine("[red]An error occurred during the protection process:[/]");
                        AnsiConsole.MarkupLine("[darkorange] > {0}[/]", error.EscapeMarkup());
                        AnsiConsole.MarkupLine("[darkorange] > The process is still active but may not finish successfully.[/]");
                        AnsiConsole.MarkupLine("[blue] > The error has been reported and notified to our team, you will soon receive news about the solution.[/]");
                    });

                    result.OnClose(hub, _ =>
                    {
                        AnsiConsole.MarkupLine("");
                        AnsiConsole.MarkupLine("[lime]Protection has ended. [/]");
                        semaphore.Release();
                    });

                    semaphore.WaitOne();
                });
        }
    }
}