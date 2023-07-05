// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Builder to configure a command line application.
/// </summary>
public interface ICommandArgsBuilder
{
    /// <summary>
    /// Adds a new command by leveraging a <see cref="ICommandBuilder"/>.
    /// </summary>
    /// <returns>The builder to configure the new command.</returns>
    ICommandBuilder AddCommand();
}