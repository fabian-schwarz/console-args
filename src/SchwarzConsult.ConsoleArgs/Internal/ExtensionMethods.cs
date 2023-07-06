using System;
using System.Collections.Generic;
using System.Linq;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal static class ExtensionMethods
{
    public static Argument? FindArgumentByName(this List<Argument> globalArguments, string value)
        => globalArguments.Find(a => a.Name?.ToLowerInvariant() == value.ToLowerInvariant());
    public static Argument? FindArgumentByAbbreviation(this List<Argument> globalArguments, string value)
        => globalArguments.Find(a => a.Abbreviation?.ToLowerInvariant() == value.ToLowerInvariant());

    public static List<TKey> FindDuplicates<TSource, TKey>(this IEnumerable<TSource> collection,
        Func<TSource, TKey> keySelector)
    {
        return collection
            .GroupBy(keySelector)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
    }
    
    public static List<string> FindDuplicatesButRemoveEmptyStrings<TSource>(this IEnumerable<TSource> collection,
        Func<TSource, string> keySelector)
    {
        return collection
            .Where(p => !string.IsNullOrWhiteSpace(keySelector(p)))
            .GroupBy(keySelector)
            .Where(g => g.Count() > 1)
            .Select(g => g.Key)
            .ToList();
    }
}