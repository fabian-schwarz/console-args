using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Builder to configure a command line application.
/// </summary>
public interface ICommandArgsBuilder
{
    /// <summary>
    /// Enables or disables the default help command.
    /// </summary>
    /// <param name="isEnabled">Flag if default help is enabled or disabled.</param>
    /// <param name="name">The name for the default help argument.</param>
    /// <param name="abbreviation">The abbreviation for the default help command.</param>
    /// <returns>The command line application builder.</returns>
    ICommandArgsBuilder AddDefaultHelp(bool isEnabled = true, string name = "help", string abbreviation = "?");
    /// <summary>
    /// Adds a new command by leveraging a <see cref="ICommandBuilder"/>.
    /// </summary>
    /// <returns>The builder to configure the new command.</returns>
    ICommandBuilder AddCommand();
    /// <summary>
    /// Adds a new global argument.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The command line application builder.</returns>
    public ICommandArgsBuilder AddGlobalArgument(ArgumentKeys keys, string? description,
        Func<string?, Task<ValidationResult>>? validator = default);
    /// <summary>
    /// Adds a new global argument.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Abbreviation of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The command line application builder.</returns>
    public ICommandArgsBuilder AddGlobalArgument(string name, string abbreviation, string? description,
        Func<string?, Task<ValidationResult>>? validator = default);
    /// <summary>
    /// Adds a new global argument.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <param name="validator">Optional validator function to validate the input value from the user.</param>
    /// <returns>The command line application builder.</returns>
    public ICommandArgsBuilder AddGlobalArgument(string name, string? description,
        Func<string?, Task<ValidationResult>>? validator);
    /// <summary>
    /// Adds a new global argument.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The command line application builder.</returns>
    public ICommandArgsBuilder AddGlobalArgument(string name, string? description);
    /// <summary>
    /// Sets the default handler if no command specific handler was found
    /// </summary>
    /// <typeparam name="THandler">Type of the handler. Must Implement <see cref="ICommandHandler"/>.</typeparam>
    /// <returns>The command line application builder.</returns>
    ICommandArgsBuilder SetDefaultHandler<THandler>()
        where THandler : ICommandHandler;
    /// <summary>
    /// Sets the default handler if no command specific handler was found
    /// </summary>
    /// <param name="delegateHandler">Handler implementation.</param>
    /// <returns>The command line application builder.</returns>
    ICommandArgsBuilder SetDefaultHandler(Func<ICommandArgumentsBag, Task> delegateHandler);
    /// <summary>
    /// Adds a global switch argument to the application.
    /// </summary>
    /// <param name="keys">Keys of the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The command line application builder.</returns>
    ICommandArgsBuilder AddGlobalSwitchArgument(ArgumentKeys keys, string? description = "");
    /// <summary>
    /// Adds a global switch argument to the application.
    /// </summary>
    /// <param name="name">Name of the new argument.</param>
    /// <param name="abbreviation">Optional abbreviation for the new argument.</param>
    /// <param name="description">Optional description for the new argument.</param>
    /// <returns>The command line application builder.</returns>
    ICommandArgsBuilder AddGlobalSwitchArgument(string name, string? abbreviation = "", string? description = "");
}