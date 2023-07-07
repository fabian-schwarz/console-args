using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandArgsBuilder : ICommandArgsBuilder
{
    private readonly List<CommandBuilder> _commands;
    private readonly List<Argument> _globalArguments;
    private readonly DefaultHelp _defaultHelp;
    private Type? _defaultHandler;
    private Func<ICommandArgumentsBag, Task>? _defaultDelegateHandler;

    public CommandArgsBuilder()
    {
        this._commands = new List<CommandBuilder>();
        this._globalArguments = new List<Argument>();
        this._defaultHelp = new();
        this._defaultHandler = default;
        this._defaultDelegateHandler = default;
    }

    public ICommandBuilder AddCommand()
    {
        var builder = new CommandBuilder(null);
        this._commands.Add(builder);
        return builder;
    }

    public CommandArgs Build() => new()
    {
        Commands = this._commands.Select(c => c.Build()).ToList(),
        GlobalArguments = this._globalArguments,
        DefaultHelp = this._defaultHelp,
        DefaultHandler = this._defaultHandler,
        DefaultDelegateHandler = this._defaultDelegateHandler,
    };
    
    public ICommandArgsBuilder SetDefaultHandler<THandler>()
        where THandler : ICommandHandler
    {
        this._defaultHandler = typeof(THandler);
        this._defaultDelegateHandler = default;
        return this;
    }
    
    public ICommandArgsBuilder SetDefaultHandler(Func<ICommandArgumentsBag, Task> delegateHandler)
    {
        Guard.ThrowIfNull(delegateHandler);

        this._defaultDelegateHandler = delegateHandler;
        this._defaultHandler = default;
        return this;
    }

    public ICommandArgsBuilder AddDefaultHelp(bool isEnabled = true, string name = "help", string abbreviation = "?")
    {
        this._defaultHelp.IsEnabled = isEnabled;
        this._defaultHelp.Name = name;
        this._defaultHelp.Abbreviation = abbreviation;
        return this;
    }
    
    public ICommandArgsBuilder AddGlobalArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<bool>>? validator = default) 
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation, description, false, validator);
    
    public ICommandArgsBuilder AddGlobalArgument(string name, string abbreviation, string? description,
        Func<string?, Task<bool>>? validator = default) => this.AddArgument(name, abbreviation, description, false, validator);
    
    public ICommandArgsBuilder AddGlobalArgument(string name, string? description,
        Func<string?, Task<bool>>? validator) => this.AddArgument(name, string.Empty, description, false, validator);

    public ICommandArgsBuilder AddGlobalArgument(string name, string? description) 
        => this.AddArgument(name, string.Empty, description);

    private ICommandArgsBuilder AddArgument(string name, string? abbreviation = "", string? description = "", bool isRequired = false,
        Func<string?, Task<bool>>? validator = default)
    {
        this._globalArguments.Add(new Argument
        {
            Name = name,
            Abbreviation = abbreviation,
            Description = description,
            IsRequired = isRequired,
            Validator = validator,
            IsSwitch = false
        });
        return this;
    }
}