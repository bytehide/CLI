using MatthiWare.CommandLine.Abstractions.Command;
using Shield.Client;
using Shield.Client.Extensions;
using Shield.Client.Models;
using Shield.Client.Models.API;
using Shield.Client.Models.API.Application;
using Shield.Client.Models.API.Project;
using ShieldCLI.Models;
using ShieldCLI.Models.Config;
using ShieldCLI.Repos;
using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShieldCLI.Commands.Config
{
    public class MakeConfigCommand : Command<GlobalOptions, MakeConfigOptions>
    {
        private ClientManager ClientManager { get; set; }

        public MakeConfigCommand(ClientManager clientManager)
        {
            ClientManager = clientManager;
        }
        public override void OnConfigure(ICommandConfigurationBuilder builder)
        {
            builder.Name("config:make").Description("Create a config settings file");
        }

        public override void OnExecute(GlobalOptions option, MakeConfigOptions options)
        {


            if (!ClientManager.HasValidClient())
            {

                AnsiConsole.Markup("[red]NOT logged in. \nYou must be logged in to use .[/]");
                return;
            };

            try
            {
                var type = options.Type;
                var path = options.Path;
                var preset = options.Preset;
                string[] protectionsId = { };
                var name = options.Name;


                if (type != "application" && type != "project")
                {
                    type = AnsiConsole.Prompt(
                             new SelectionPrompt<string>()
                             .Title("[white]Please choose the type of protection[/]?")
                             .PageSize(3)
                             .AddChoice("project")
                             .AddChoice("application"));
                }




                if (preset != "maximum" && preset != "balance" && preset != "custom" && preset != "optimized")
                {
                    preset = AnsiConsole.Prompt(
                                 new SelectionPrompt<string>()
                                 .Title("[white]Please choose the preset for the protection of protection[/]?")
                                 .PageSize(4)
                                 .AddChoice("maximum")
                                 .AddChoice("balance")
                                 .AddChoice("optimized")
                                 .AddChoice("custom"));
                }

                var shieldConfig = ShieldConfiguration.CreateInstance();

                //shieldConfig.MakeProjectCustomConfiguration(protectionsId);


                //shieldConfig.MakeApplicationCustomConfiguration(protectionsId);




                ProjectConfigurationDto projectConfig = null;
                ApplicationConfigurationDto applicationConfig = null;

                if (type == "application")
                {
                    applicationConfig = preset.Equals("custom") ? shieldConfig.MakeApplicationCustomConfiguration(protectionsId) :
                        shieldConfig.MakeApplicationConfiguration(ShieldConfigurationPresets.ToPreset(preset));

                    ConfigurationExtensions.SaveToFile(applicationConfig, path, name);
                }
                else
                {
                    projectConfig = preset.Equals("custom") ? shieldConfig.MakeProjectCustomConfiguration(protectionsId) :
                        shieldConfig.MakeProjectConfiguration(ShieldConfigurationPresets.ToPreset(preset));

                    ConfigurationExtensions.SaveToFile(applicationConfig, path, name);

                }




                AnsiConsole.Markup("[lime]Configuration file created sucessfully.[/]");
            }
            catch
            {

            }


        }
    }

}