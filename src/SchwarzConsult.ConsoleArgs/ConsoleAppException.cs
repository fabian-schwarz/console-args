using System.Runtime.Serialization;
using SchwarzConsult.ConsoleArgs.Internal;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// Exception that is thrown by the console app.
/// </summary>
[Serializable]
public class ConsoleAppException : Exception
{
    /// <summary>
    /// Initializes a new instance of the see <see cref="ConsoleAppException"/> class.
    /// </summary>
    /// <param name="message">Error message.</param>
    public ConsoleAppException(string message) : base(message)
    {
        Guard.ThrowIfNullOrWhiteSpace(message);
    }
    
    protected ConsoleAppException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}