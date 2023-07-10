using System;
using System.Globalization;
using System.IO;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public sealed class ParseTests
{
    [Fact]
    public void ItShouldParseAsGuid()
    {
        // Arrange
        var value = Guid.NewGuid();
        
        // Act
        var actual = Parse.AsGuid(value.ToString());
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsGuid()
    {
        // Arrange & Act
        void Act() => Parse.AsGuid("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsFileInfo()
    {
        var file = Path.GetTempFileName();
        try
        {
            // Arrange, Act & Assert
            Assert.NotNull(Parse.AsFile(file));
        }
        finally
        {
            try
            {
                File.Delete(file);
            }
            catch
            {
                // Empty on purpose.
            }
        }
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsFileInfo()
    {
        // Arrange && Act
        void Act() => Parse.AsFile("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsDirectoryInfo()
    {
        var directory = Path.GetTempPath();
        try
        {
            // Arrange, Act & Assert
            Assert.NotNull(Parse.AsDirectory(directory));
        }
        finally
        {
            try
            {
                File.Delete(directory);
            }
            catch
            {
                // Empty on purpose.
            }
        }
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsDirectoryInfo()
    {
        // Arrange && Act
        void Act() => Parse.AsDirectory("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsBool()
    {
        // Arrange
        var value = true;
        
        // Act
        var actual = Parse.AsBoolean(value.ToString());
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsBool()
    {
        // Arrange & Act
        void Act() => Parse.AsBoolean("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsInt()
    {
        // Arrange
        var value = 3;
        
        // Act
        var actual = Parse.AsInt(value.ToString());
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsInt()
    {
        // Arrange & Act
        void Act() => Parse.AsInt("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsLong()
    {
        // Arrange
        var value = 3L;
        
        // Act
        var actual = Parse.AsLong(value.ToString());
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsLong()
    {
        // Arrange & Act
        void Act() => Parse.AsLong("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsFloat()
    {
        // Arrange
        var value = 1f;
        
        // Act
        var actual = Parse.AsFloat(value.ToString(CultureInfo.InvariantCulture));
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsFloat()
    {
        // Arrange & Act
        void Act() => Parse.AsFloat("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsDouble()
    {
        // Arrange
        var value = 1d;
        
        // Act
        var actual = Parse.AsDouble(value.ToString(CultureInfo.InvariantCulture));
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsDouble()
    {
        // Arrange & Act
        void Act() => Parse.AsDouble("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsDecimal()
    {
        // Arrange
        var value = new Decimal(3);
        
        // Act
        var actual = Parse.AsDecimal(value.ToString(CultureInfo.InvariantCulture));
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsDecimal()
    {
        // Arrange & Act
        void Act() => Parse.AsDecimal("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsDateTime()
    {
        // Arrange
        var value = DateTime.UnixEpoch;
        
        // Act
        var actual = Parse.AsDateTime(value.ToString(CultureInfo.InvariantCulture));
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsDateTime()
    {
        // Arrange & Act
        void Act() => Parse.AsDateTime("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldParseAsTimeSpan()
    {
        // Arrange
        var value = TimeSpan.MaxValue;
        
        // Act
        var actual = Parse.AsTimeSpan(value.ToString());
        
        // Assert
        Assert.Equal(value, actual);
    }
    
    [Fact]
    public void ItShouldThrowIfCouldNotParseAsTimeSpan()
    {
        // Arrange & Act
        void Act() => Parse.AsTimeSpan("asd");
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
}