using System;
using System.Collections.Generic;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal class Command
{
    public string? Verb { get; set; }
    public string? Description { get; set; }
    public List<Argument>? Arguments { get; set; }
    public List<Command>? SubCommands { get; set; }
    public Type? Handler { get; set; }

    public override string ToString()
    {
        return this.Verb ?? string.Empty;
    }
}