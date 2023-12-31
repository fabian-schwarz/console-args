# SchwarzConsult.ConsoleArgs

A library to ease the usage of command line arguments for .NET console applications

## Installation

You should install with NuGet:
```bash
Install-Package SchwarzConsult.ConsoleArgs
```

Or via the .NET command line interface:
```bash
dotnet add package SchwarzConsult.ConsoleArgs
```
Either commands, from Package Manager Console or .NET Core CLI, will download and install SchwarzConsult.ConsoleArgs and all required dependencies.

## Usage

Both the new and old console project template are supported.

You can find a sample project in the repository subfolder **sample**.

Add a new implementation of the **IConsoleAppConfiguration** interface to your project.
This class will be used to configure the console application. 

You have access to the **IConfiguration** object to read configuration values from the *appsettings.json* and *appsettings.Development.json* files if present, user secrets if present and environment variables.

Using the **ConfigureServices** method you can register custom services to the dependency injection container for usage in your command handlers. The command handlers themself are automatically registered as Singletons.

Using the **ConfigureCommands** method you can configure the commands and their arguments.

```csharp
public class ConsoleConfiguration : IConsoleAppConfiguration
{
    public IConfiguration Configuration { get; set; } = null!;
    public IServiceCollection ConfigureServices(IServiceCollection services)
    {
        return services;
    }

    public ICommandArgsBuilder ConfigureCommands(ICommandArgsBuilder app)
    {
        app.AddGlobalArgument("subscription",
            "Name or ID of subscription. You can configure the default subscription using az account set -s NAME_OR_ID.");
        app.AddGlobalArgument("output", "o", "Output format.");
        app.AddGlobalSwitchArgument("debug", "d", "Increase logging verbosity to show all debug logs.");
        
        app.AddDefaultHelp(isEnabled: true, "help", "?");
        
        app.SetDefaultHandler(_ =>
        {
            Console.WriteLine("No command was specified. Please use --help to get a list with all possible commands");
            return Task.CompletedTask;
        });
        
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
                    "Location. Values from: az account list-locations.")
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
                    v =>
                    {
                        var isValid = new List<string>
                        {
                            "Microsoft.Compute/virtualMachineScaleSets",
                            "Microsoft.Compute/virtualMachines"
                        }.Contains(v ?? string.Empty);
                        if (isValid) return ValidationResult.OkAsync();
                        return ValidationResult.ErrorAsync($"Value '{v}' is not a valid force deletion type.");
                    })
                .SetHandler(argumentsBag =>
                {
                    foreach (var value in argumentsBag.List())
                    {
                        Console.WriteLine($"{value.Name}, {value.Abbreviation}: {value.Value}");
                    }

                    return Task.CompletedTask;
                })
                .AddSwitchArgument("no-wait", description: "Do not wait for the long-running operation to finish.")
                .AddSwitchArgument("yes", "y", "Do not prompt for confirmation.")
                .Done();
    }
}
```

Configure your command handlers by either implementing the **ICommandHandler** interface and reference them in the **SetHandler<>()** call.

```csharp
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
```

or adding them inline using a lambda expression.

```csharp
.SetHandler(argumentsBag =>
{
    // Loop through all available arguments and their values
    foreach (var value in argumentsBag.List())
    {
        Console.WriteLine($"{value.Name}, {value.Abbreviation}: {value.Value}");
    }
    
    // Get a specific argument value & parse it to a specific type
    var idFound = argumentsBag.TryGetValueByAbbreviationOrNameAs("id", "i", Parse.AsGuid, 
        out var id);
    Console.WriteLine($"Id found: {id}");

    return Task.CompletedTask;
})
```

In your **Program.cs** file you can then use the **ConsoleApp** to run your console application.

```csharp
// See https://aka.ms/new-console-template for more information

using SchwarzConsult.ConsoleArgsConsoleTest;

Console.WriteLine("Hello, World!");

await ConsoleApp.Run<ConsoleConfiguration>(args);
```

Using the example application from above you can find also in the **sample** subfolder, you can now run the following command:
```bash
./SchwarzConsult.ConsoleArgsConsoleTest group create -l westeurope -n mytestgroup
```

And you will receive the following output using the **GroupCreateHandler** class:
```bash
Hello, World!
location, l: westeurope
name, n: mytestgroup

```