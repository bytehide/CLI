using MatthiWare.CommandLine;
using ShieldCLI.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using ShieldCLI.Repos;
using Microsoft.Extensions.Logging;
using ShieldCLI.Helpers;

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

            services.AddSingleton<NugetResolver>();

            services.AddSingleton<DependenciesResolver>();

            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<Consumer>();

            services.AddSingleton<ClientManager>();

            services.AddCommandLineParser<GlobalOptions>(options);

            await using var provider = services.BuildServiceProvider();

            var app = provider.GetService<Consumer>();
          
            await app!.Run(args);
        }
    }
}
