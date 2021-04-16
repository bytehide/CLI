using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.Logging;
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

        private ClientManager ClientManager { get; set; }
        public Consumer(ILogger<Consumer> logger, ICommandLineParser<GlobalOptions> commandLineParser, ClientManager clientManager)
        {
            _logger = logger;
            ClientManager = clientManager;
            CommandLineParser = commandLineParser;
        }

        internal async Task Run(string[] args)
        {

            //CommandLineParser.DiscoverCommands(Assembly.GetExecutingAssembly());

            var result = await CommandLineParser.ParseAsync(args);

            if (result.HasErrors)
            {
                return;
            }
        }
    }
}
