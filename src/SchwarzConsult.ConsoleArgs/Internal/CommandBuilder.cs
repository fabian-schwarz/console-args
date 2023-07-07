using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class CommandBuilder : ICommandBuilder
{
    private readonly CommandBuilder? _parent;
    private readonly List<Argument> _arguments;
    private readonly List<Command> _subCommands;
    private string _verb;
    private string _description;
    private Type? _handler;
    private Func<ICommandArgumentsBag, Task>? _delegateHandler;

    public CommandBuilder(CommandBuilder? parent)
    {
        this._parent = parent;
        this._arguments = new List<Argument>();
        this._subCommands = new List<Command>();
        this._verb = string.Empty;
        this._description = string.Empty;
        this._handler = default;
        this._delegateHandler = default;
    }

    public Command Build()
    {
        return new Command
        {
            Verb = this._verb,
            Description = this._description,
            Arguments = this._arguments,
            SubCommands = this._subCommands,
            Handler = this._handler,
            DelegateHandler = this._delegateHandler,
        };
    }

    public ICommandBuilder Done()
    {
        if (this._parent is null)
        {
            throw new ArgumentException("No parent command builder found, you are probably on root level already");
        }
        
        this._parent._subCommands.Add(new Command
        {
            Verb = this._verb,
            Description = this._description,
            Arguments = this._arguments,
            SubCommands = this._subCommands,
            Handler = this._handler,
            DelegateHandler = this._delegateHandler,
        });
        return this._parent;
    }

    public ICommandBuilder AddSubCommand()
    {
        return new CommandBuilder(this);
    }

    public ICommandBuilder SetHandler<THandler>()
        where THandler : ICommandHandler
    {
        this._handler = typeof(THandler);
        this._delegateHandler = default;
        return this;
    }
    
    public ICommandBuilder SetHandler(Func<ICommandArgumentsBag, Task> delegateHandler)
    {
        Guard.ThrowIfNull(delegateHandler);

        this._delegateHandler = delegateHandler;
        this._handler = default;
        return this;
    }

    public ICommandBuilder SetVerb(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        
        this._verb = value;
        return this;
    }
    
    public ICommandBuilder SetDescription(string value)
    {
        Guard.ThrowIfNullOrWhiteSpace(value);
        
        this._description = value;
        return this;
    }
    
    public ICommandBuilder AddRequiredArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<bool>>? validator)
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation ?? string.Empty, description, true, validator);
    
    public ICommandBuilder AddRequiredArgument(string name, string abbreviation, string? description,
        Func<string?, Task<bool>>? validator) => this.AddArgument(name, abbreviation, description, true, validator);
    
    public ICommandBuilder AddRequiredArgument(ArgumentKeys keys, string? description)
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation ?? string.Empty, description, true);
    
    public ICommandBuilder AddRequiredArgument(string name, string abbreviation, string? description)
        => this.AddArgument(name, abbreviation, description, true);
    
    public ICommandBuilder AddOptionalArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<bool>>? validator = default) 
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation, description, false, validator);
    
    public ICommandBuilder AddOptionalArgument(string name, string abbreviation, string? description,
        Func<string?, Task<bool>>? validator = default) => this.AddArgument(name, abbreviation, description, false, validator);
    
    public ICommandBuilder AddOptionalArgument(string name, string? description,
        Func<string?, Task<bool>>? validator) => this.AddArgument(name, string.Empty, description, false, validator);

    public ICommandBuilder AddOptionalArgument(string name, string? description) 
        => this.AddArgument(name, string.Empty, description);

    public ICommandBuilder AddArgument(ArgumentKeys keys, string? description = "", bool isRequired = false,
        Func<string?, Task<bool>>? validator = default)
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation, description, isRequired, validator);
    
    public ICommandBuilder AddArgument(string name, string? abbreviation = "", string? description = "", bool isRequired = false,
        Func<string?, Task<bool>>? validator = default)
    {
        this._arguments.Add(new Argument
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
    
    public ICommandBuilder AddSwitchArgument(ArgumentKeys keys, string? description = "")
        => this.AddSwitchArgument(keys.Name ?? string.Empty, keys.Abbreviation, description);
    
    public ICommandBuilder AddSwitchArgument(string name, string? abbreviation = "", string? description = "")
    {
        this._arguments.Add(new Argument
        {
            Name = name,
            Abbreviation = abbreviation,
            Description = description,
            IsRequired = false,
            Validator = default,
            IsSwitch = true
        });
        return this;
    }
}