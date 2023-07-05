using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// A handler for a command.
/// </summary>
public interface ICommandHandler
{
    /// <summary>
    /// Handles the command.
    /// </summary>
    /// <param name="argumentsBag">Collection of argument names, abbreviations and their corresponding values.</param>
    /// <returns>Task object.</returns>
    public Task Handle(ICommandArgumentsBag argumentsBag);
}