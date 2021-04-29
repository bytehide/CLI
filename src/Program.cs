using System;
using System.Threading.Tasks;
using Dotnetsafer.CLI.Commands;
using Dotnetsafer.CLI.Commands.App;
using Dotnetsafer.CLI.Commands.Auth;
using Dotnetsafer.CLI.Commands.Config;
using Dotnetsafer.CLI.Commands.Project;
using Dotnetsafer.CLI.Commands.Protect;
using Dotnetsafer.CLI.Helpers;
using Dotnetsafer.CLI.Repos;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Spectre.Console.Cli;

namespace Dotnetsafer.CLI
{
    public class Program
    {
        private static async Task Main(string[] args)
        {

            Console.Title = "Dotnetsafer CLI";

            var services = new ServiceCollection();

            services.AddSingleton<NugetResolver>();

            services.AddSingleton<DependenciesResolver>();

            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.None)).AddTransient<Program>();

            services.AddSingleton<ClientManager>();

            services.AddSingleton<ShieldCommands>();

            var registrar = new TypeRegistrar(services);

            var dotnetsafer = new CommandApp(registrar);


            dotnetsafer.Configure(config =>
            {
                //Dotnetsafer commands:
                config.AddCommand<HelloCommand>("hello").IsHidden();

                config.AddCommand<AuthLoginCommand>("login").WithDescription("Log into your dotnetsafer account.");
                config.AddCommand<AuthRegisterCommand>("register").WithDescription("Sign up for dotnetsafer.");
                config.AddCommand<AuthClearCommand>("clear").WithDescription("Delete your stored dotnetsafer credentials.");
                config.AddCommand<AuthCheckCommand>("check").WithDescription("Check if your stored credentials are valid.");

                //Dotnetsafer Shield Commands:
                config.AddBranch<ShieldSettings>("shield", shield =>
                {

                    shield.SetDescription("All the functions that dotnetsafer shield offers.");
                    shield.AddCommand<ProtectCommand>("protect").WithDescription("Protect your application with a single execution, ideal for configuring your scripts and automating msbuilds.");
                    shield.AddCommand<ProtectAutoCommand>("protect:auto").WithDescription("Protect your application with an interactive flow that will ask you for parameters in real time.");
                    shield.AddCommand<ProjectGetCommand>("project:find").WithDescription("Search for a project in Shield");
                    shield.AddCommand<ProjectCreateCommand>("project:make").WithDescription("Create a new project in Shield");
                    shield.AddCommand<AppAddCommand>("application:add").WithDescription("Upload an application to a project in Shield");
                    shield.AddCommand<ConfigGetCommand>("config:find").WithDescription("Search for a config file in Shield");
                    shield.AddCommand<ConfigMakeCommand>("config:make").WithDescription("Make a config file to use in Shield protection");

                });
            });

            await dotnetsafer.RunAsync(args);
        }
    }
}
