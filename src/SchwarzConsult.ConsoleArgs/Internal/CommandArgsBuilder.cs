using System;
using System.Collections.Generic;
using System.Linq;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandArgsBuilder : ICommandArgsBuilder
{
    private readonly List<CommandBuilder> _commands;

    public CommandArgsBuilder()
    {
        this._commands = new List<CommandBuilder>();
    }

    public ICommandBuilder AddCommand()
    {
        var builder = new CommandBuilder(null);
        this._commands.Add(builder);
        return builder;
    }

    public List<Command> Build() => this._commands.Select(c => c.Build()).ToList();
}