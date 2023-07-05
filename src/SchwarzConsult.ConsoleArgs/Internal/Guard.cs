using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal static class Guard
{
    [DebuggerStepThrough]
    public static void ThrowIfNull<TObj>([NotNull] TObj? obj,
        [CallerArgumentExpression("obj")] string? paramName = null) where TObj : class
    {
        if (obj is null) Throw(paramName);
    }

    [DebuggerStepThrough]
    public static void ThrowIfNullOrWhiteSpace([NotNull] string? value,
        [CallerArgumentExpression("value")] string? paramName = null)
    {
        if (string.IsNullOrWhiteSpace(value)) Throw(paramName);
    }

    [DoesNotReturn]
    private static void Throw(string? paramName)
    {
        throw new ArgumentNullException(paramName);
    }
}