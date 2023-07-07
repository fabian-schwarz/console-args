using System.Collections.Generic;
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
        var commandArgs = commandsBuilder.Build();
        if (!commandArgs.Commands.Any() && 
            commandArgs.DefaultHandler is null && 
            commandArgs.DefaultDelegateHandler is null) return;
            
        // Validate verbs being unique on each level & arg names / abbreviations also unique on command
        var validationService = new ValidationService();
        var validationResult = validationService.ValidateUserConfiguration(commandArgs);
        if (!validationResult.IsValid) throw new ConsoleAppException(validationResult.ErrorMessage!);

        // Register the command handlers
        var commandService = new CommandService();
        commandService.RegisterCommandHandlers(commandArgs.Commands, services);

        // Build the command hierarchy to find the command to execute
        var commandHierarchy = commandService.ExtractCommandHierarchy(commandArgs.Commands, args);
        if (!commandHierarchy.Any()) commandHierarchy.Add(new Command()); // We add an empty command to support the default handler
        var command = commandHierarchy[^1];
            
        // Extract the values from the command line arguments
        var values = commandService.ExtractArgumentValuesForCommand(commandArgs.GlobalArguments,
            command, args, commandArgs.DefaultHelp);

        // Validate if input values fill the required arguments configured & pass user configured validators
        validationResult = await validationService.ValidateInputValues(command, values);
        if (!validationResult.IsValid) throw new ConsoleAppException(validationResult.ErrorMessage!);

        // Find out what to run and run it
        await RunDefaultHelpOrHandlers(commandArgs, values, commandHierarchy, command, services).ConfigureAwait(false);
    }

    private static async Task RunDefaultHelpOrHandlers(CommandArgs commandArgs, ICommandArgumentsBag values,
        List<Command> commandHierarchy, Command command, IServiceCollection services)
    {
        if (commandArgs.DefaultHelp.IsEnabled &&
            values.TryGetValueByAbbreviationOrName(commandArgs.DefaultHelp.Name, commandArgs.DefaultHelp.Abbreviation,
                out _))
        {
            // Run the default help command
            var outputWriter = new OutputWriter();
            outputWriter.WriteDefaultHelp(commandHierarchy, commandArgs.GlobalArguments);
        }
        else
        {
            await RunHandler(commandArgs, command, values, services).ConfigureAwait(false);
        }
    }

    private static async Task RunHandler(CommandArgs commandArgs, Command command, ICommandArgumentsBag values, IServiceCollection services)
    {
        // Check and run handler if command handler exists
        var serviceProvider = services.BuildServiceProvider();
        if (command.DelegateHandler is not null)
        {
            await command.DelegateHandler(values).ConfigureAwait(false);
            return;
        }
        
        if (command.Handler is not null)
        {
            var handler = serviceProvider.GetService(command.Handler);
            if (handler is ICommandHandler commandHandler)
            {
                await commandHandler.Handle(values).ConfigureAwait(false);
                return;
            }
        }
        
        // Check and run default handler if command handler does not exist
        if (commandArgs.DefaultDelegateHandler is not null)
        {
            await commandArgs.DefaultDelegateHandler(values).ConfigureAwait(false);
            return;
        }
        
        if (commandArgs.DefaultHandler is not null)
        {
            var handler = serviceProvider.GetService(commandArgs.DefaultHandler);
            if (handler is ICommandHandler commandHandler)
            {
                await commandHandler.Handle(values).ConfigureAwait(false);
                return;
            }
        }
        
        throw new ConsoleAppException($"No handler found for command with verb '{command.Verb}', and no default handler configured.");
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