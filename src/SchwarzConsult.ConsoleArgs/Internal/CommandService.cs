using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandService
{
    public void RegisterCommandHandlers(List<Command> commands, IServiceCollection services)
    {
        Guard.ThrowIfNull(commands);
        Guard.ThrowIfNull(services);
        
        foreach (var command in commands)
        {
            if (command.Handler is not null)
            {
                services.TryAddSingleton(command.Handler);
            }

            if (command.SubCommands is not null)
            {
                RegisterCommandHandlers(command.SubCommands, services);
            }
        }
    }
    
    public List<Command> ExtractCommandHierarchy(List<Command> commands, string[] args)
    {
        Guard.ThrowIfNull(commands);
        Guard.ThrowIfNull(args);
        
        var commandHierarchy = new List<Command>();
        var currentCommands = commands;
        for (var i = 0; i < args.Length; i++)
        {
            var item = args[i];
            if (item.StartsWith("--") || item.StartsWith('-'))
            {
                i++;
                continue;
            }
                
            var command = currentCommands.Find(c => 
                !string.IsNullOrWhiteSpace(c.Verb) && 
                c.Verb.Equals(item, StringComparison.InvariantCultureIgnoreCase));
            if (command is null) throw new ConsoleAppException($"Command '{item}' not found");
                
            commandHierarchy.Add(command);
            currentCommands = command.SubCommands ?? new List<Command>();
        }

        return commandHierarchy;
    }

    public ICommandArgumentsBag ExtractArgumentValuesForCommand(List<Argument> globalArguments, Command command, 
        string[] args, DefaultHelp defaultHelp)
    {
        Guard.ThrowIfNull(globalArguments);
        Guard.ThrowIfNull(command);
        Guard.ThrowIfNull(args);
        
        var result = new CommandArgumentsBag();
        for (int i = 0; i < args.Length; i++)
        {
            var item = args[i];
            var helper = new ArgHelper(item);
            if (!helper.Found) continue;
            
            // For switch values, we don't need to check for the next argument and we need to check if we get out of bounds
            if (i + 1 == args.Length)
            {
                // The last one seems to be a possible switch argument
                this.FindArgumentAndAddIfFound(helper, globalArguments, command, item, result, defaultHelp);
            }
            else
            {
                var argumentValue = args[i + 1];
                this.FindArgumentAndAddIfFound(helper, globalArguments, command, argumentValue, result, defaultHelp);
                // Only skip the next argument if it is not a switch argument
                var isArg = argumentValue.StartsWith("--") || argumentValue.StartsWith('-');
                if (!isArg) i++;
            }
        }

        return result;
    }

    private void FindArgumentAndAddIfFound(ArgHelper argHelper, List<Argument> globalArguments, Command command, 
        string argumentValue, CommandArgumentsBag result, DefaultHelp defaultHelp)
    {
        if (this.AddDefaultHelpIfEnabledAndFound(defaultHelp, argHelper, result)) return;

        var globalArgument = argHelper.IsAbbreviation ? 
            globalArguments.FindArgumentByAbbreviation(argHelper.Name ?? string.Empty) : 
            globalArguments.FindArgumentByName(argHelper.Name ?? string.Empty);
        if (globalArgument is not null)
        {
            result.Add(new ArgumentValue(globalArgument.Name ?? string.Empty, globalArgument.Abbreviation, argumentValue));
        }
        else
        {
            if (command.Arguments is not null)
            {
                var argument = argHelper.IsAbbreviation ? 
                    command.Arguments.FindArgumentByAbbreviation(argHelper.Name ?? string.Empty) : 
                    command.Arguments.FindArgumentByName(argHelper.Name ?? string.Empty);
                if (argument is not null)
                {
                    result.Add(new ArgumentValue(argument.Name ?? string.Empty, argument.Abbreviation, argumentValue));
                }
            }
        }
    }

    private bool AddDefaultHelpIfEnabledAndFound(DefaultHelp defaultHelp, ArgHelper argHelper, CommandArgumentsBag result)
    {
        if (defaultHelp.IsEnabled)
        {
            if (argHelper.IsAbbreviation)
            {
                if (defaultHelp.Abbreviation.Equals(argHelper.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(new ArgumentValue(defaultHelp.Name, defaultHelp.Abbreviation, true.ToString()));
                    return true;
                }
            }
            else
            {
                if (defaultHelp.Name.Equals(argHelper.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    result.Add(new ArgumentValue(defaultHelp.Name, defaultHelp.Abbreviation, true.ToString()));
                    return true;
                }
            }
        }

        return false;
    }
    
    private sealed class ArgHelper
    {
        public ArgHelper(string value)
        {
            Guard.ThrowIfNullOrWhiteSpace(value);
            
            if (value.StartsWith("--"))
            {
                this.SetFoundAsName(value.Substring(2));
            }
            else
            {
                if (value.StartsWith('-'))
                {
                    this.SetFoundAsAbbreviation(value.Substring(1));
                }
            }
        }
        
        public bool Found { get; private set; }
        public string? Name { get; private set; } = string.Empty;
        public bool IsAbbreviation { get; private set; }
        
        private void SetFoundAsName(string name)
        {
            this.Found = true;
            this.Name = name;
            this.IsAbbreviation = false;
        }
        
        private void SetFoundAsAbbreviation(string name)
        {
            this.Found = true;
            this.Name = name;
            this.IsAbbreviation = true;
        }
    }
}