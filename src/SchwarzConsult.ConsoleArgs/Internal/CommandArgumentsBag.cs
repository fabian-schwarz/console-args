using System;
using System.Collections.Generic;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal class CommandArgumentsBag : ICommandArgumentsBag
{
    private readonly List<ArgumentValue> _argumentValues;
    
    public CommandArgumentsBag()
    {
        this._argumentValues = new List<ArgumentValue>();
    }

    public void Add(ArgumentValue value)
    {
        Guard.ThrowIfNull(value);
        
        this.ThrowIfAlreadyIncluded(value);
        this._argumentValues.Add(value);
    }
    
    public void Add(string name, string? abbreviation, string? value)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);
        
        this.Add(new ArgumentValue(name, abbreviation, value));
    }

    public IEnumerable<ArgumentValue> List() => new List<ArgumentValue>(this._argumentValues);
    
    public bool TryGetValue(ArgumentKeys keys, out string? value)
    {
        Guard.ThrowIfNull(keys);
        
        return this.TryGetValueByAbbreviationOrName(keys.Name ?? string.Empty, keys.Abbreviation ?? string.Empty, out value);
    }

    public bool TryGetValueByAbbreviationOrName(string name, string abbreviation, out string? value)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);
        Guard.ThrowIfNullOrWhiteSpace(abbreviation);
        
        var result = this.TryGetValueByName(name, out value);
        if (result) return true;
        
        return this.TryGetValueByAbbreviation(abbreviation, out value);
    }
    
    public bool TryGetValueByName(string name, out string? value)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);
        
        var argument = this._argumentValues.Find(a => a.Name == name);
        if (argument is not null)
        {
            value = argument.Value;
            return true;
        }

        value = null;
        return false;
    }

    public bool TryGetValueByAbbreviation(string abbreviation, out string? value)
    {
        Guard.ThrowIfNullOrWhiteSpace(abbreviation);
        
        var argument = this._argumentValues.Find(a => a.Abbreviation == abbreviation);
        if (argument is not null)
        {
            value = argument.Value;
            return true;
        }

        value = null;
        return false;
    }

    private void ThrowIfAlreadyIncluded(ArgumentValue value)
    {
        var existing = this._argumentValues.Exists(a => a.Name == value.Name && a.Abbreviation == value.Abbreviation);
        if (existing)
            throw new ConsoleAppException(
                $"Argument with name '{value.Name}' and abbreviation '{value.Abbreviation}' is already defined.");
    }
}