// ReSharper disable once CheckNamespace

using System.Threading.Tasks;

namespace System;

public sealed class ValidationResult
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

    public static Task<ValidationResult> OkAsync() => Task.FromResult(Ok());
    public static Task<ValidationResult> ErrorAsync(string errorMessage) => Task.FromResult(Error(errorMessage));

    public override string ToString()
    {
        return $"IsValid: {this.IsValid}, ErrorMessage: {this.ErrorMessage ?? string.Empty}";
    }
}