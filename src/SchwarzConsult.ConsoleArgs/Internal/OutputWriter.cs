using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class OutputWriter
{
    private const string Tab = "    ";
    public void WriteDefaultHelp(List<Command> commandHierarchy, List<Argument> globalArguments)
    {
        Guard.ThrowIfNull(commandHierarchy);
        Guard.ThrowIfNull(globalArguments);
        
        var command = commandHierarchy[^1];
        var verbs = string.Join(" ", commandHierarchy.Select(c => c.Verb));
        var description = string.IsNullOrWhiteSpace(command.Description) ? string.Empty : $" : {command.Description}";
        
        Console.WriteLine();
        Console.WriteLine("Command");
        Console.WriteLine($"{Tab}{verbs}{description}");
        if (command.Arguments is not null && command.Arguments.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Arguments");
            foreach (var argument in command.Arguments)
            {
                this.WriteArgument(argument);
            }
        }
        if (globalArguments.Count > 0)
        {
            Console.WriteLine();
            Console.WriteLine("Global Arguments");
            foreach (var argument in globalArguments)
            {
                this.WriteArgument(argument);
            }
        }
    }

    private void WriteArgument(Argument argument)
    {
        var builder = new StringBuilder();
        builder.Append($"{Tab}--{argument.Name}");
        if (!string.IsNullOrWhiteSpace(argument.Abbreviation)) builder.Append($" -{argument.Abbreviation}");
        if (argument.IsRequired) builder.Append(" [Required]");
        if (!string.IsNullOrWhiteSpace(argument.Description)) builder.Append($" : {argument.Description}");
        Console.WriteLine(builder.ToString());
    }
}