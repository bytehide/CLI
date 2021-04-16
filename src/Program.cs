using MatthiWare.CommandLine;
using ShieldCLI.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ShieldCLI.Repos;
using Microsoft.Extensions.Logging;
using ShieldCLI.Commands.Project;
using ShieldCLI.Commands;

namespace ShieldCLI
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var options = new CommandLineParserOptions
            {
                AppName = "ds"
            };

            var services = new ServiceCollection();

            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<Consumer>();

            services.AddSingleton<ClientManager>();

            services.AddSingleton<ShieldCommands>();

            services.AddCommandLineParser<GlobalOptions>(options);


            await using var provider = services.BuildServiceProvider();

            var app = provider.GetService<Consumer>();

            await app!.Run(args);
        }
    }
}
