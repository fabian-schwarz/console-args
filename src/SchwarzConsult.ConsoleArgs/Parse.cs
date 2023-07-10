using System.IO;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Parses the argument value to the desired type.
/// </summary>
public static class Parse
{
    /// <summary>
    /// Parses the value to type <see cref="Guid"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static Guid AsGuid(string value)
    {
        if (Guid.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Guid");
    }

    /// <summary>
    /// Parses the value to type <see cref="FileInfo"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static FileInfo AsFile(string value)
    {
        if (File.Exists(value)) return new FileInfo(value);

        throw new ConsoleAppException($"Could not parse value '{value}' to type FileInfo");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="DirectoryInfo"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static DirectoryInfo AsDirectory(string value)
    {
        if (Directory.Exists(value)) return new DirectoryInfo(value);

        throw new ConsoleAppException($"Could not parse value '{value}' to type DirectoryInfo");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="bool"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static bool AsBoolean(string value)
    {
        if (bool.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Boolean");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="int"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static int AsInt(string value)
    {
        if (int.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Int");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="long"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static long AsLong(string value)
    {
        if (long.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Long");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="float"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static float AsFloat(string value)
    {
        if (float.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Float");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="double"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static double AsDouble(string value)
    {
        if (double.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Double");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="Decimal"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static Decimal AsDecimal(string value)
    {
        if (Decimal.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type Decimal");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="DateTime"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static DateTime AsDateTime(string value)
    {
        if (DateTime.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type DateTime");
    }
    
    /// <summary>
    /// Parses the value to type <see cref="TimeSpan"/>. Throws an exception if the value could not be parsed.
    /// </summary>
    /// <param name="value">Value to parse</param>
    /// <returns>Parsed value.</returns>
    /// <exception cref="ConsoleAppException">If the value could not be parsed.</exception>
    public static TimeSpan AsTimeSpan(string value)
    {
        if (TimeSpan.TryParse(value, out var result)) return result;

        throw new ConsoleAppException($"Could not parse value '{value}' to type TimeSpan");
    }
}