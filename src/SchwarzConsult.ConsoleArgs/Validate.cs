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
    public static Task<ValidationResult> AsBoolean(string? value)
    {
        if (bool.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Bool.");
    }
    
    /// <summary>
    /// Validates the argument value as int.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsInt(string? value)
    {
        if (int.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Int.");
    }

    /// <summary>
    /// Validates the argument value as long.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsLong(string? value)
    {
        if (long.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Long.");
    }

    /// <summary>
    /// Validates the argument value as float.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsFloat(string? value)
    {
        if (float.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Float.");
    }

    /// <summary>
    /// Validates the argument value as double.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsDouble(string? value) 
    {
        if (Double.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Double.");
    }

    /// <summary>
    /// Validates the argument value as DateTime.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsDateTime(string? value)
    {
        if (DateTime.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid DateTime.");
    }

    /// <summary>
    /// Validates the argument value as TimeSpan.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsTimeSpan(string? value)
    {
        if (TimeSpan.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid TimeSpan.");
    }

    /// <summary>
    /// Validates the argument value as decimal.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsDecimal(string? value)
    {
        if (Decimal.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Decimal.");
    }

    /// <summary>
    /// Validates the argument value as Guid.
    /// </summary>
    /// <param name="value">Value to validate.</param>
    /// <returns>True if validation was successful, else false.</returns>
    public static Task<ValidationResult> AsGuid(string? value)
    {
        if (Guid.TryParse(value, out _)) return ValidationResult.OkAsync();
        
        return ValidationResult.ErrorAsync($"Value '{value}' is not a valid Guid.");
    }
}