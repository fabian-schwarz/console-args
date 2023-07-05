using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SchwarzConsult.ConsoleArgsConsoleTest;

public class ConsoleConfiguration : IConsoleAppConfiguration
{
    public IConfiguration Configuration { get; set; } = null!;
    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services;
    }

    public ICommandArgsBuilder ConfigureCommands(ICommandArgsBuilder app)
    {
        this.GroupCommands(app.AddCommand());
        return app;
    }

    private void GroupCommands(ICommandBuilder command)
    {
        command
            .SetVerb("group")
            .SetDescription("Manage resource groups")
            .AddSubCommand()
                .SetVerb("create")
                .SetDescription("Create a new resource group.")
                .AddRequiredArgument("location", "l",
                    "Location. Values from: az account list-locations. You can configure the default location using az configure --defaults location=<location>.")
                .AddRequiredArgument("name", "n", "Name of the new resource group.")
                .AddOptionalArgument("managed-by", "The ID of the resource that manages this resource group.")
                .AddOptionalArgument("tags",
                    "Space-separated tags: key[=value] [key[=value] ...]. Use '' to clear existing tags.")
                .SetHandler<GroupCreateHandler>()
                .Done()
            .AddSubCommand()
                .SetVerb("delete")
                .SetDescription("Delete a resource group.")
                .AddRequiredArgument("name", "n", "The name of the resource group to delete.")
                .AddOptionalArgument("force-deletion-types", "f", "The resource types you want to force delete.",
            v => Task.FromResult(new List<string>
                    {
                        "Microsoft.Compute/virtualMachineScaleSets",
                        "Microsoft.Compute/virtualMachines"
                    }.Contains(v ?? string.Empty)))
                .SetHandler<GroupDeleteHandler>()
                .AddSwitchArgument("no-wait", description: "Do not wait for the long-running operation to finish.")
                .AddSwitchArgument("yes", "y", "Do not prompt for confirmation.")
                .Done();
    }

    private class GroupCreateHandler : ICommandHandler
    {
        public Task Handle(ICommandArgumentsBag argumentsBag)
        {
            foreach (var value in argumentsBag.List())
            {
                Console.WriteLine($"{value.Name}, {value.Abbreviation}: {value.Value}");
            }

            return Task.CompletedTask;
        }
    }
    
    private class GroupDeleteHandler : ICommandHandler
    {
        public Task Handle(ICommandArgumentsBag argumentsBag)
        {
            foreach (var value in argumentsBag.List())
            {
                Console.WriteLine($"{value.Name}, {value.Abbreviation}: {value.Value}");
            }

            return Task.CompletedTask;
        }
    }
}