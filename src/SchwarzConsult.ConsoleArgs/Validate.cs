using System.Threading.Tasks;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Validate functions for the command line arguments.
/// </summary>
public static class Validate
{
    /// <summary>
    /// Validates the argument value as boolean.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsBoolean(string? value)
        => Task.FromResult(bool.TryParse(value, out _));
    
    /// <summary>
    /// Validates the argument value as int.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsInt(string? value)
        => Task.FromResult(int.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as long.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsLong(string? value)
        => Task.FromResult(long.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as float.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsFloat(string? value)
        => Task.FromResult(float.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as double.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsDouble(string? value) 
        => Task.FromResult(double.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as DateTime.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsDateTime(string? value)
        => Task.FromResult(DateTime.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as TimeSpan.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsTimeSpan(string? value)
        => Task.FromResult(TimeSpan.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as decimal.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsDecimal(string? value)
        => Task.FromResult(Decimal.TryParse(value, out _));

    /// <summary>
    /// Validates the argument value as Guid.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<bool> AsGuid(string? value)
        => Task.FromResult(Guid.TryParse(value, out _));
}