using MatthiWare.CommandLine;
using ShieldCLI.Commands;
using ShieldCLI.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using SecureLocalStorage;
using ShieldCLI.Repos;

namespace ShieldCLI
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var options = new CommandLineParserOptions
            {
                AppName = "ds"
            };

            var services = new ServiceCollection();

            services.AddScoped<KeyManager>();

            services.AddCommandLineParser<GlobalOptions>(options);

            var provider = services.BuildServiceProvider();

            var parser = provider.GetRequiredService<ICommandLineParser<GlobalOptions>>();

            parser.DiscoverCommands(Assembly.GetExecutingAssembly());

            var result = await parser.ParseAsync(args);

            if (result.HasErrors)
            {
                return;
            }

            if (result.Result.Verbose)
            {
                Console.WriteLine("Verbose specified!");
            }
        }
    }
}
