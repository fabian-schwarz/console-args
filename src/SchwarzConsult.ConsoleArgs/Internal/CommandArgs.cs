using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandArgs
{
    public List<Command> Commands { get; init; } = new();
    public List<Argument> GlobalArguments { get; init; } = new();
    public DefaultHelp DefaultHelp { get; init; } = new();
    public Type? DefaultHandler { get; set; }
    public Func<ICommandArgumentsBag, Task>? DefaultDelegateHandler { get; set; }
}