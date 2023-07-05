using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class ValidationService
{
    public ValidationResult ValidateDuplicationsRecursive(List<Command> commands)
    {
        Guard.ThrowIfNull(commands);
        
        // Ensure unique verbs
        var duplicateVerbs = commands
            .GroupBy(p => p.Verb)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicateVerbs.Count > 0) return ValidationResult.Error($"Verb '{duplicateVerbs[0]}' is not unique on a command group");

        foreach (var command in commands)
        {
            if (command.Arguments is not null)
            {
                var validateArguments = this.ValidateArgumentsDuplications(command);
                if (!validateArguments.IsValid) return validateArguments;
            }
            
            if (command.SubCommands is not null)
            {
                var subResult = this.ValidateDuplicationsRecursive(command.SubCommands);
                if (!subResult.IsValid) return subResult;
            }
        }

        return ValidationResult.Ok();
    }

    public ValidationResult ValidateRequiredArgumentsSet(Command command, ICommandArgumentsBag bag)
    {
        Guard.ThrowIfNull(command);
        Guard.ThrowIfNull(bag);
        
        if (command.Arguments is null) return ValidationResult.Ok();
        
        foreach (var argument in command.Arguments)
        {
            if (!argument.IsRequired) continue;
            
            if (!bag.TryGetValueByName(argument.Name!, out _))
            {
                return ValidationResult.Error($"Required argument '{argument.Name}' is missing");
            }
        }
        
        return ValidationResult.Ok();
    }
    
    public async Task<ValidationResult> ValidateArgumentValues(Command command, ICommandArgumentsBag bag)
    {
        Guard.ThrowIfNull(command);
        Guard.ThrowIfNull(bag);
        
        if (command.Arguments is null) return ValidationResult.Ok();
        
        foreach (var argument in command.Arguments)
        {
            if (!bag.TryGetValueByName(argument.Name!, out var value)) continue;
            
            if (argument.Validator is not null)
            {
                var isValid = await argument.Validator(value).ConfigureAwait(false);
                if (!isValid)
                {
                    return ValidationResult.Error($"Argument value '{value}' for argument with name '{argument.Name}' is invalid!");
                }
            }
        }
        
        return ValidationResult.Ok();
    }
    
    private ValidationResult ValidateArgumentsDuplications(Command command)
    {
        if (command.Arguments is not null)
        {
            var duplicateArgumentNames = command
                .Arguments
                .GroupBy(p => p.Name)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicateArgumentNames.Count > 0) 
                return ValidationResult.Error($"Argument name '{duplicateArgumentNames[0]}' is not unique on command with verb '{command.Verb}'");
                
            var duplicateArgumentAbbreviations = command
                .Arguments
                .Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation))
                .GroupBy(p => p.Abbreviation)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicateArgumentAbbreviations.Count > 0) 
                return ValidationResult.Error($"Argument abbreviation '{duplicateArgumentAbbreviations[0]}' is not unique on command with verb '{command.Verb}'");
        }

        return ValidationResult.Ok();
    }

    public ValidationResult ValidateGlobalArgumentsDoNotOverlapRecursive(List<Command> commands, List<Argument> globalArguments)
    {
        Guard.ThrowIfNull(commands);
        Guard.ThrowIfNull(globalArguments);
        
        foreach (var command in commands)
        {
            if (command.Arguments is null) continue;
            
            var commandArgNames = command.Arguments.Select(a => a.Name);
            var globalArgNames = globalArguments.Select(a => a.Name);
            var names = commandArgNames.Concat(globalArgNames);
            var duplicateNames = names.GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicateNames.Count > 0) 
                return ValidationResult.Error($"Argument name '{duplicateNames[0]}' is duplicated on command with verb '{command.Verb}' and a global argument");
            
            var commandArgAbbreviations = command.Arguments.Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation)).Select(a => a.Abbreviation);
            var globalArgAbbreviations = globalArguments.Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation)).Select(a => a.Abbreviation);
            var abbreviations = commandArgAbbreviations.Concat(globalArgAbbreviations);
            var duplicateAbbreviations = abbreviations.GroupBy(p => p)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (duplicateAbbreviations.Count > 0) 
                return ValidationResult.Error($"Argument abbreviation '{duplicateAbbreviations[0]}' is duplicated on command with verb '{command.Verb}' and a global argument");

            if (command.SubCommands is not null)
            {
                var subResult = this.ValidateGlobalArgumentsDoNotOverlapRecursive(command.SubCommands, globalArguments);
                if (!subResult.IsValid) return subResult;
            }
        }

        return ValidationResult.Ok();
    }
    
    public ValidationResult ValidateGlobalArgumentsUnique(List<Argument> globalArguments)
    {
        Guard.ThrowIfNull(globalArguments);
        
        var names = globalArguments.Select(a => a.Name);
        var duplicateNames = names.GroupBy(p => p)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicateNames.Count > 0) 
            return ValidationResult.Error($"Argument name '{duplicateNames[0]}' is duplicated on global arguments");
            
        var abbreviations = globalArguments.Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation)).Select(a => a.Abbreviation);
        var duplicateAbbreviations = abbreviations.GroupBy(p => p)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
        if (duplicateAbbreviations.Count > 0) 
            return ValidationResult.Error($"Argument abbreviation '{duplicateAbbreviations[0]}' is duplicated on global arguments");

        return ValidationResult.Ok();
    }
}