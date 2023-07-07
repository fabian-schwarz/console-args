using System;
using System.Threading.Tasks;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class ValidateTests
{
    [Theory]
    [InlineData("true", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsBoolean(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsBoolean(value)).IsValid);
    }
    
    [Theory]
    [InlineData("12345", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsInt(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsInt(value)).IsValid);
    }
    
    [Theory]
    [InlineData("12345", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsLong(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsLong(value)).IsValid);
    }
    
    [Theory]
    [InlineData("123.4567", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsFloat(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsFloat(value)).IsValid);
    }
    
    [Theory]
    [InlineData("123.4567", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsDouble(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsDouble(value)).IsValid);
    }
    
    [Theory]
    [InlineData("2021-03-31", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsDateTime(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsDateTime(value)).IsValid);
    }
    
    [Theory]
    [InlineData("123.4567", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsDecimal(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsDecimal(value)).IsValid);
    }
    
    [Theory]
    [InlineData("41788f5e-e37e-45f8-a1f8-506f483695f2", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsGuid(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsGuid(value)).IsValid);
    }
    
    [Theory]
    [InlineData("6.12:14:45", true)]
    [InlineData("test", false)]
    public async Task ItShouldValidateAsTimeSpan(string value, bool result)
    {
        // Arrange, Act & Assert
        Assert.Equal(result, (await Validate.AsTimeSpan(value)).IsValid);
    }
}