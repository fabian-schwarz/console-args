using SchwarzConsult.ConsoleArgs.Internal;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Stores the name, abbreviation and value for an argument.
/// </summary>
public class ArgumentValue : ArgumentKeys
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentValue"/> class.
    /// </summary>
    /// <param name="name">Name for this argument.</param>
    /// <param name="abbreviation">Abbreviation for this argument</param>
    /// <param name="value">Value for this argument.</param>
    public ArgumentValue(string name, string? abbreviation, string? value)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);
        
        this.Name = name;
        this.Abbreviation = abbreviation;
        this.Value = value;
    }
    
    /// <summary>
    /// Gets the value for this argument.
    /// </summary>
    public string? Value { get; }

    public override string ToString()
    {
        return $"Name: {this.Name}, Abbreviation: {this.Abbreviation ?? string.Empty}, Value: {this.Value ?? string.Empty}";
    }
}