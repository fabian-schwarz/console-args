namespace SchwarzConsult.ConsoleArgs.Internal;

internal sealed class ValidationResult
{
    private ValidationResult(bool isValid, string? errorMessage)
    {
        this.IsValid = isValid;
        this.ErrorMessage = errorMessage;
    }
    
    public bool IsValid { get; }
    public string? ErrorMessage { get; }
    
    public static ValidationResult Ok() => new (true, null);
    public static ValidationResult Error(string errorMessage) => new (false, errorMessage);

    public override string ToString()
    {
        return $"IsValid: {this.IsValid}, ErrorMessage: {this.ErrorMessage ?? string.Empty}";
    }
}