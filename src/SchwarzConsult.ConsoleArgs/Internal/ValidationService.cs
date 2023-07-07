using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class ValidationService
{
    public ValidationResult ValidateUserConfiguration(CommandArgs commandArgs)
    {
        Guard.ThrowIfNull(commandArgs);
        
        // Validate if the verbs on the same level are unique
        // Validate if the argument names & abbreviations for a command are unique
        var validationResult = this.ValidateDuplicationsRecursive(commandArgs.Commands);
        if (!validationResult.IsValid) return validationResult;
        
        // Validate if the global arguments are unique
        validationResult = this.ValidateGlobalArgumentsUnique(commandArgs.GlobalArguments);
        if (!validationResult.IsValid) return validationResult;
        
        // Validate if the global arguments do not overlap with the command arguments
        validationResult = this.ValidateGlobalArgumentsDoNotOverlapRecursive(commandArgs.Commands, 
            commandArgs.GlobalArguments);
        if (!validationResult.IsValid) return validationResult;
        
        return ValidationResult.Ok();
    }

    public async Task<ValidationResult> ValidateInputValues(Command command, ICommandArgumentsBag values)
    {
        Guard.ThrowIfNull(command);
        Guard.ThrowIfNull(values);
        
        // Validate if required arguments are set
        var validationResult = this.ValidateRequiredArgumentsSet(command, values);
        if (!validationResult.IsValid) return validationResult;
            
        // Validate argument values (e.g. if a string is a valid int) if validator is set
        validationResult = await this.ValidateArgumentValues(command, values).ConfigureAwait(false);
        if (!validationResult.IsValid) return validationResult;

        return await ValidationResult.OkAsync();
    }
    
    public ValidationResult ValidateDuplicationsRecursive(List<Command> commands)
    {
        Guard.ThrowIfNull(commands);
        
        // Ensure unique verbs
        var duplicateVerbs = commands.FindDuplicates(p => p.Verb);
        if (duplicateVerbs.Count > 0) return ValidationResult.Error($"Verb '{duplicateVerbs[0]}' is not unique on a command group");

        foreach (var command in commands)
        {
            var validateArguments = this.ValidateDuplicationsRecursiveForCommand(command);
            if (!validateArguments.IsValid) return validateArguments;
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
        
        if (command.Arguments is null) return await ValidationResult.OkAsync();
        
        foreach (var argument in command.Arguments)
        {
            if (!bag.TryGetValueByName(argument.Name!, out var value)) continue;
            
            if (argument.Validator is not null)
            {
                var isValid = await argument.Validator(value).ConfigureAwait(false);
                if (!isValid.IsValid)
                {
                    return await ValidationResult.ErrorAsync($"Argument value '{value}' for argument with name '{argument.Name}' is invalid: {isValid.ErrorMessage}");
                }
            }
        }
        
        return await ValidationResult.OkAsync();
    }

    public ValidationResult ValidateGlobalArgumentsDoNotOverlapRecursive(List<Command> commands, List<Argument> globalArguments)
    {
        Guard.ThrowIfNull(commands);
        Guard.ThrowIfNull(globalArguments);
        
        foreach (var command in commands)
        {
            var validationResult = this.ValidateGlobalArgumentsDoNotOverlapRecursiveForCommand(command, globalArguments);
            if (!validationResult.IsValid) return validationResult;
        }

        return ValidationResult.Ok();
    }

    public ValidationResult ValidateGlobalArgumentsUnique(List<Argument> globalArguments)
    {
        Guard.ThrowIfNull(globalArguments);

        var duplicateNames = globalArguments.FindDuplicates(g => g.Name);
        if (duplicateNames.Count > 0) 
            return ValidationResult.Error($"Argument name '{duplicateNames[0]}' is duplicated on global arguments");

        var duplicateAbbreviations =
            globalArguments.FindDuplicatesButRemoveEmptyStrings(g => g.Abbreviation ?? string.Empty);
        if (duplicateAbbreviations.Count > 0) 
            return ValidationResult.Error($"Argument abbreviation '{duplicateAbbreviations[0]}' is duplicated on global arguments");

        return ValidationResult.Ok();
    }
    
    private ValidationResult ValidateArgumentsDuplications(Command command)
    {
        if (command.Arguments is not null)
        {
            var duplicateArgumentNames = command
                .Arguments
                .FindDuplicates(p => p.Name);
            if (duplicateArgumentNames.Count > 0) 
                return ValidationResult.Error($"Argument name '{duplicateArgumentNames[0]}' is not unique on command with verb '{command.Verb}'");

            var duplicateArgumentAbbreviations = command
                .Arguments
                .FindDuplicatesButRemoveEmptyStrings(p => p.Abbreviation ?? string.Empty);
            if (duplicateArgumentAbbreviations.Count > 0) 
                return ValidationResult.Error($"Argument abbreviation '{duplicateArgumentAbbreviations[0]}' is not unique on command with verb '{command.Verb}'");
        }

        return ValidationResult.Ok();
    }
    
    private ValidationResult ValidateGlobalArgumentsDoNotOverlapRecursiveForCommand(Command command,
        List<Argument> globalArguments)
    {
        if (command.Arguments is null) return ValidationResult.Ok();
            
        var commandArgNames = command.Arguments.Select(a => a.Name);
        var globalArgNames = globalArguments.Select(a => a.Name);
        var names = commandArgNames.Concat(globalArgNames);
        var duplicateNames = names.FindDuplicates(p => p);
        if (duplicateNames.Count > 0) 
            return ValidationResult.Error($"Argument name '{duplicateNames[0]}' is duplicated on command with verb '{command.Verb}' and a global argument");
            
        var commandArgAbbreviations = command.Arguments.Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation)).Select(a => a.Abbreviation);
        var globalArgAbbreviations = globalArguments.Where(p => !string.IsNullOrWhiteSpace(p.Abbreviation)).Select(a => a.Abbreviation);
        var abbreviations = commandArgAbbreviations.Concat(globalArgAbbreviations);
        var duplicateAbbreviations = abbreviations.FindDuplicates(p => p);
        if (duplicateAbbreviations.Count > 0) 
            return ValidationResult.Error($"Argument abbreviation '{duplicateAbbreviations[0]}' is duplicated on command with verb '{command.Verb}' and a global argument");

        if (command.SubCommands is not null)
        {
            var subResult = this.ValidateGlobalArgumentsDoNotOverlapRecursive(command.SubCommands, globalArguments);
            if (!subResult.IsValid) return subResult;
        }
        
        return ValidationResult.Ok();
    }
    
    private ValidationResult ValidateDuplicationsRecursiveForCommand(Command command)
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
        
        return ValidationResult.Ok();
    }
}