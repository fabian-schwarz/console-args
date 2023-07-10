using System.Collections.Generic;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Stores the arguments and their values for a command.
/// </summary>
public interface ICommandArgumentsBag
{
    /// <summary>
    /// Tries to get a value by its argument keys.
    /// </summary>
    /// <param name="keys">Argument keys to get the value for.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <returns>True if the argument was found by its name, else false.</returns>
    bool TryGetValue(ArgumentKeys keys, out string? value);
    /// <summary>
    /// Tries to get a value by its argument name.
    /// </summary>
    /// <param name="name">Name of the argument to get the value for.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <returns>True if the argument was found by its name, else false.</returns>
    bool TryGetValueByName(string name, out string? value);
    /// <summary>
    /// Tries to get a value by its argument name and parses it to the specified type.
    /// </summary>
    /// <param name="name">Name of the argument to get the value for.</param>
    /// <param name="parser">Function to parse the value. See <see cref="Parse"/> class for default implementations.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <typeparam name="TResult">Type of the parsed result.</typeparam>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueByNameAs<TResult>(string name, Func<string, TResult> parser, out TResult? value);
    /// <summary>
    /// Tries to get a value by its argument abbreviation.
    /// </summary>
    /// <param name="abbreviation">Abbreviation of the argument to get the value for.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueByAbbreviation(string abbreviation, out string? value);
    /// <summary>
    /// Tries to get a value by its argument name. If the argument was not found by its name, it tries to find it by its abbreviation.
    /// </summary>
    /// <param name="name">Name of the argument to get the value for.</param>
    /// <param name="abbreviation">Abbreviation of the argument to get the value for.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueByAbbreviationOrName(string name, string abbreviation, out string? value);
    /// <summary>
    /// Tries to get a value by its argument name. If the argument was not found by its name, it tries to find it by its abbreviation  and parses it to the specified type.
    /// </summary>
    /// <param name="name">Name of the argument to get the value for.</param>
    /// <param name="abbreviation">Abbreviation of the argument to get the value for.</param>
    /// <param name="parser">Function to parse the value. See <see cref="Parse"/> class for default implementations.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueByAbbreviationOrNameAs<TResult>(string name, string abbreviation,
        Func<string, TResult> parser, out TResult? value);
    /// <summary>
    /// Tries to get a value by its argument name. If the argument was not found by its name, it tries to find it by its abbreviation  and parses it to the specified type.
    /// </summary>
    /// <param name="keys">Argument keys to get the value for.</param>
    /// <param name="parser">Function to parse the value. See <see cref="Parse"/> class for default implementations.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <typeparam name="TResult"></typeparam>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueAs<TResult>(ArgumentKeys keys, Func<string, TResult> parser,
        out TResult? value);
    /// <summary>
    /// Gets a list with all argument values.
    /// </summary>
    /// <returns>A list with all argument values.</returns>
    IEnumerable<ArgumentValue> List();
    /// <summary>
    /// Tries to get a value by its argument abbreviation and parses it to the specified type.
    /// </summary>
    /// <param name="abbreviation">Abbreviation of the argument to get the value for.</param>
    /// <param name="parser">Function to parse the value. See <see cref="Parse"/> class for default implementations.</param>
    /// <param name="value">Returns the value for the argument if found.</param>
    /// <typeparam name="TResult">Type of the parsed result.</typeparam>
    /// <returns>True if the argument was found by its abbreviation, else false.</returns>
    bool TryGetValueByAbbreviationAs<TResult>(string abbreviation, Func<string, TResult> parser, out TResult? value);
}