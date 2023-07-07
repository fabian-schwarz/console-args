using System;
using System.Threading.Tasks;

namespace SchwarzConsult.ConsoleArgs.Internal;

internal class Argument
{
    public string? Name { get; set; }
    public string? Abbreviation { get; set; }
    public string? Description { get; set; }
    public bool IsRequired { get; set; }
    public Func<string?, Task<ValidationResult>>? Validator { get; set; }
    public bool IsSwitch { get; set; }
    
    public override string ToString()
    {
        return this.Name ?? string.Empty;
    }
}