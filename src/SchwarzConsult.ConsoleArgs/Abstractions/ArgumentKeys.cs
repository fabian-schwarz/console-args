using SchwarzConsult.ConsoleArgs.Internal;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Holds the keys (name and abbreviation) for an argument.
/// </summary>
public class ArgumentKeys
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentKeys"/> class.
    /// </summary>
    public ArgumentKeys()
    {
        this.Name = string.Empty;
        this.Abbreviation = string.Empty;
    }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentKeys"/> class.
    /// </summary>
    /// <param name="name">Name of the argument.</param>
    /// <param name="abbreviation">Abbreviation of the argument.</param>
    public ArgumentKeys(string name, string abbreviation)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);
        Guard.ThrowIfNullOrWhiteSpace(abbreviation);
        
        this.Name = name;
        this.Abbreviation = abbreviation;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ArgumentKeys"/> class.
    /// </summary>
    /// <param name="name">Name of the argument.</param>
    public ArgumentKeys(string name)
    {
        Guard.ThrowIfNullOrWhiteSpace(name);

        this.Name = name;
        this.Abbreviation = string.Empty;
    }
    
    /// <summary>
    /// Gets or sets the name of the argument.
    /// </summary>
    public string? Name { get; init; }
    /// <summary>
    /// Gets or sets the abbreviation of the argument.
    /// </summary>
    public string? Abbreviation { get; init; }

    public override string ToString()
    {
        return $"Name: {this.Name ?? string.Empty}, Abbreviation: {this.Abbreviation ?? string.Empty}";
    }
}