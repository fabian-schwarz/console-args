using System.Collections.Generic;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandArgs
{
    public List<Command> Commands { get; init; } = new();
    public List<Argument> GlobalArguments { get; init; } = new();
    public DefaultHelp DefaultHelp { get; init; } = new();
}