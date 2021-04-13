using MatthiWare.CommandLine;
using ShieldCLI.Models;
using System;
using System.Reflection;
using System.Threading.Tasks;
using MatthiWare.CommandLine.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ShieldCLI.Repos;
using Microsoft.Extensions.Logging;

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

            services.AddLogging(configure => configure.AddConsole().SetMinimumLevel(LogLevel.Debug))
                .AddTransient<Consumer>();

            services.AddScoped<KeyManager>();

            services.AddTransient((provider) =>
            {
                return Shield.Client.ShieldClient.CreateInstance("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjA5ZmUzOGY3LWM0MGYtNGFjNy04YTJmLTc4MDQxYWFlZWFmYyIsInVuaXF1ZV9uYW1lIjoiOTMwOGVhZGMtMTc5NC00NzE3LWFiY2YtZTM5ZjVhM2FmMDQ3IiwidmVyc2lvbiI6IjEuMC4wIiwic2VydmljZSI6ImRvdG5ldHNhZmVyIiwiZWRpdGlvbiI6ImNvbW11bml0eSIsImp0aSI6IjFiODAyOGI0LWU0MmItNDZkNi04MmQwLTE3MDQwZTY4Y2Q3YyIsImV4cCI6MTYxODM0MTM2MH0.CudOBJRQUwnFzB_OpJtUEWdxwA_vHOvsIoYkk8BZzzQ");
            });

            services.AddCommandLineParser<GlobalOptions>(options);

            await using var provider = services.BuildServiceProvider();

            var app = provider.GetService<Consumer>();
          
            await app?.Run(args);
        }
    }
}
