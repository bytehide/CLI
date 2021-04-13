using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.Logging;
using Shield.Client;
using ShieldCLI.Models;
using ShieldCLI.Repos;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ShieldCLI
{
    public class Consumer
    {
        private readonly ILogger _logger;
        private ICommandLineParser<GlobalOptions> CommandLineParser { get; set; }

        private ShieldClient ShieldClient { get; set; }
        public Consumer(ILogger<Consumer> logger, ICommandLineParser<GlobalOptions> commandLineParser, ShieldClient shieldClient)
        {
            _logger = logger;
            ShieldClient = shieldClient;
            CommandLineParser = commandLineParser;
        }

        internal async Task Run(string[] args)
        {
            CommandLineParser.DiscoverCommands(Assembly.GetExecutingAssembly());

            var result = await CommandLineParser.ParseAsync(args);

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
