
﻿using System;
using System.Collections.Generic;
using System.IO;
using ShieldCLI.Commands;

using ShieldCLI.Models;
using ShieldCLI.Models.App;
using ShieldCLI.Repos;
using Spectre.Console;
using System.Linq;
using System.Threading.Tasks;
using ShieldCLI.Helpers;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.App
{
    public class AddAppCommand : Command<GlobalOptions, AddAppOptions>
    {
        private ClientManager ClientManager { get; set; }

        private ShieldCommands ShieldCommands { get; set; }

        public AddAppCommand(ClientManager clientManager, ShieldCommands shieldCommands)
        {
            ClientManager = clientManager;
            ShieldCommands = shieldCommands;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("application:add").Description("Add an application to a project");
        }


        public override void OnExecute(GlobalOptions option, AddAppOptions options)
        {
            //if (!ClientManager.HasValidClient())
            //{
            //    AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
            //    return;
            //};

            try
            {
                var run = Task.Run(async () =>
                {
                    var (isValid, requiredDependencies, (module, createdContext)) =
                        await DependenciesResolver.GetAssemblyInfoAsync(options.Path ?? string.Empty);

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

            //try
            //{
            //    var keyproject = options.KeyProject;
            //    var path = options.Path;
            //    List<string> dependenciesPaths = Directory.GetFiles(Path.GetDirectoryName(path)).ToList();

            //    ShieldCommands.saludoShield();

            //    var appUpload = ClientManager.Client.Application.UploadApplicationDirectly(keyproject, path, dependenciesPaths);

            //}
            //catch
            //{
            //    AnsiConsole.Write("Error");
            //}

                        _ = DependenciesResolver.GetUnresolved(module,
                            createdContext, requiredDep, userPath.Select(Path.GetDirectoryName).ToArray());
                    }


                    return requiredDep;
                });
                
                var resolved = AsyncHelpers.RunSync(async () => await run);
            }
            catch (Exception ex)
            {
                AnsiConsole.WriteException(ex);
            }

        }


        
        //TODO: @jespanag  Create method to get projects on Client
    }

}
