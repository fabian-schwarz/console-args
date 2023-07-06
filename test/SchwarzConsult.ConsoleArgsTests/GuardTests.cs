using System;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class GuardTests
{
    [Fact]
    public void ItShouldThrowIfNull()
    {
        // Arrange
        object? value = null;

        // Act
        void Act() => Guard.ThrowIfNull(value);

        // Assert
        Assert.Throws<ArgumentNullException>((Action) Act);
    }
    
    [Fact]
    public void ItShouldNotThrowIfNotNull()
    {
        // Arrange
        object? value = new object();

        // Act
        void Act() => Guard.ThrowIfNull(value);
        var exception = Record.Exception(Act);

        // Assert does not throw
        Assert.Null(exception);
    }
    
    [Fact]
    public void ItShouldThrowIfNullOrWhitespace()
    {
        // Arrange
        string? value = null;

        // Act
        void Act() => Guard.ThrowIfNullOrWhiteSpace(value);

        // Assert
        Assert.Throws<ArgumentNullException>((Action) Act);
    }
    
    [Fact]
    public void ItShouldNotThrowIfNotNullOrWhitespace()
    {
        // Arrange
        string? value = "Test";

        // Act
        void Act() => Guard.ThrowIfNullOrWhiteSpace(value);
        var exception = Record.Exception(Act);

        // Assert does not throw
        Assert.Null(exception);
    }
}