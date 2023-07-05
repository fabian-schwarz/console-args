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
    
    public Argument? FindArgumentByName(string value)
        => this.Arguments?.Find(a => a.Name?.ToLowerInvariant() == value.ToLowerInvariant());
    public Argument? FindArgumentByAbbreviation(string value)
        => this.Arguments?.Find(a => a.Abbreviation?.ToLowerInvariant() == value.ToLowerInvariant());

    public override string ToString()
    {
        return this.Verb ?? string.Empty;
    }
}