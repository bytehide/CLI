using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.Logging;
using ShieldCLI.Models;
using System.Threading.Tasks;

namespace ShieldCLI
{
    public class Consumer
    {
        private readonly ILogger _logger;
        private ICommandLineParser<GlobalOptions> CommandLineParser { get; set; }
        
        public Consumer(ILogger<Consumer> logger, ICommandLineParser<GlobalOptions> commandLineParser)
        {
            _logger = logger;
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
