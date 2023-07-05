using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SchwarzConsult.ConsoleArgs.Internal;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// A console application.
/// </summary>
public static class ConsoleApp
{
    /// <summary>
    /// Runs a new console application.
    /// </summary>
    /// <param name="args">Arguments read from the command line.</param>
    /// <typeparam name="TConsoleAppConfiguration">Type to configure this console application.</typeparam>
    /// <returns>Task object.</returns>
    public static Task Run<TConsoleAppConfiguration>(string[] args)
        where TConsoleAppConfiguration : IConsoleAppConfiguration, new() =>
        Run<TConsoleAppConfiguration>(GetConfiguration(), args);
        
    /// <summary>
    /// Runs a new console application.
    /// </summary>
    /// <param name="configuration">Configuration object to use in your custom command handler services.</param>
    /// <param name="args">Arguments read from the command line.</param>
    /// <typeparam name="TConsoleAppConfiguration">Type to configure this console application.</typeparam>
    /// <returns>Task object.</returns>
    public static async Task Run<TConsoleAppConfiguration>(IConfiguration configuration, string[] args) 
        where TConsoleAppConfiguration : IConsoleAppConfiguration, new ()
    {
        Guard.ThrowIfNull(configuration);
        Guard.ThrowIfNull(args);
            
        // Build configuration and register user defined services
        var appConfiguration = new TConsoleAppConfiguration
        {
            Configuration = configuration
        };
        var services = appConfiguration.ConfigureServices(new ServiceCollection());
            
        // Build commands
        var commandsBuilder = new CommandArgsBuilder();
        appConfiguration.ConfigureCommands(commandsBuilder);
        var commands = commandsBuilder.Build();
        if (!commands.Any()) return;
            
        // Validate verbs being unique on each level & arg names / abbreviations also unique on command
        var validationService = new ValidationService();
        var validationResult = validationService.ValidateDuplicationsRecursive(commands);
        if (!validationResult.IsValid) throw new ConsoleAppException(validationResult.ErrorMessage!);
            
        // Register the command handlers
        var commandService = new CommandService();
        commandService.RegisterCommandHandlers(commands, services);

        // Build the command hierarchy to find the command to execute
        var commandHierarchy = commandService.ExtractCommandHierarchy(commands, args);
        if (!commandHierarchy.Any()) throw new ConsoleAppException("Could not build command hierarchy");
        var command = commandHierarchy[^1];
            
        // Get the values
        var values = commandService.ExtractArgumentValuesForCommand(command, args);

        // Validate if required arguments are set
        validationResult = validationService.ValidateRequiredArgumentsSet(command, values);
        if (!validationResult.IsValid) throw new ConsoleAppException(validationResult.ErrorMessage!);
            
        // Validate argument values (e.g. if a string is a valid int) if validator is set
        validationResult = await validationService.ValidateArgumentValues(command, values).ConfigureAwait(false);
        if (!validationResult.IsValid) throw new ConsoleAppException(validationResult.ErrorMessage!);

        // Get the service
        if (command.Handler is null) throw new ConsoleAppException($"No handler registered for command '{command.Verb}'");
        var serviceProvider = services.BuildServiceProvider();
        var handler = serviceProvider.GetService(command.Handler);
        if (handler is ICommandHandler commandHandler)
        {
            await commandHandler.Handle(values).ConfigureAwait(false);
        }
    }

    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: false)
            .AddJsonFile("appsettings.Development.json", optional: true)
            .AddEnvironmentVariables()
            .AddUserSecrets(typeof(ConsoleApp).Assembly, true, false)
            .Build();
    }
}