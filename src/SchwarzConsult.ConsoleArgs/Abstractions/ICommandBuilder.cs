using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Builder for a single command in a command line application.
/// </summary>
public interface ICommandBuilder
{
    /// <summary>
    /// Sets the description of the command.
    /// </summary>
    /// <param name="value">Description value.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder SetDescription(string value);
    /// <summary>
    /// Sets the verb of the command.
    /// </summary>
    /// <param name="value">Verb value.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder SetVerb(string value);
    /// <summary>
    /// Adds a new sub command by leveraging a <see cref="ICommandBuilder"/>.
    /// </summary>
    /// <returns>The command builder for the sub command.</returns>
    ICommandBuilder AddSubCommand();
    /// <summary>
    /// Registers the handler for this command. Must implement <see cref="ICommandHandler"/>. The handler is registered automatically in the DI container as singleton.
    /// </summary>
    /// <typeparam name="THandler">Type of the handler. Must Implement <see cref="ICommandHandler"/>.</typeparam>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder SetHandler<THandler>()
        where THandler : ICommandHandler;
    /// <summary>
    /// Registers the handler for this command.
    /// </summary>
    /// <param name="delegateHandler">Function to handle this command.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder SetHandler(Func<ICommandArgumentsBag, Task> delegateHandler);
    /// <summary>
    /// Adds a switch argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddSwitchArgument(string name, string? abbreviation = "", string? description = "");
    /// <summary>
    /// Adds a switch argument to the command.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddSwitchArgument(ArgumentKeys keys, string? description = "");
    /// <summary>
    /// Adds a custom argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="isRequired">Optional flag if argument is required. Defaults to false.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddArgument(string name, string? abbreviation = "", string? description = "", bool isRequired = false,
        Func<string?, Task<ValidationResult>>? validator = default);
    /// <summary>
    /// Adds a custom argument to the command.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="isRequired">Optional flag if argument is required. Defaults to false.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddArgument(ArgumentKeys keys, string? description = "", bool isRequired = false,
        Func<string?, Task<ValidationResult>>? validator = default);
    /// <summary>
    /// Adds a required argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    public ICommandBuilder AddRequiredArgument(string name, string abbreviation, string? description,
        Func<string?, Task<ValidationResult>>? validator);
    /// <summary>
    /// Adds a required argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The builder for the command.</returns>
    public ICommandBuilder AddRequiredArgument(string name, string abbreviation, string? description);
    /// <summary>
    /// Adds a required argument to the command.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddRequiredArgument(ArgumentKeys keys, string? description);
    /// <summary>
    /// Adds a required argument to the command.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddRequiredArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<ValidationResult>>? validator);
    /// <summary>
    /// Adds an optional argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddOptionalArgument(string name, string abbreviation, string? description,
        Func<string?, Task<ValidationResult>>? validator = default);
    /// <summary>
    /// Adds an optional argument to the command.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddOptionalArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<ValidationResult>>? validator = default) 
        => this.AddArgument(keys.Name ?? string.Empty, keys.Abbreviation, description, false, validator);
    /// <summary>
    /// Adds an optional argument to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The builder for the command.</returns>
    ICommandBuilder AddOptionalArgument(string name, string? description,
        Func<string?, Task<ValidationResult>>? validator);
    /// <summary>
    /// Adds an optional command to the command.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The builder for the command.</returns>
    public ICommandBuilder AddOptionalArgument(string name, string? description);
    /// <summary>
    /// Marks the configuration of the current sub command as done and returns the parent command builder. Throws an exception if the current command is the root command.
    /// </summary>
    /// <returns>The parent command builder.</returns>
    ICommandBuilder Done();
}