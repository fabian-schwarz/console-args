using System.Collections.Generic;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal static class ExtensionMethods
{
    public static Argument? FindArgumentByName(this List<Argument> globalArguments, string value)
        => globalArguments.Find(a => a.Name?.ToLowerInvariant() == value.ToLowerInvariant());
    public static Argument? FindArgumentByAbbreviation(this List<Argument> globalArguments, string value)
        => globalArguments.Find(a => a.Abbreviation?.ToLowerInvariant() == value.ToLowerInvariant());   
}