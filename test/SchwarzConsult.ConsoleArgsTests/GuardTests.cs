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
        void act() => Guard.ThrowIfNull(value);

        // Assert
        Assert.Throws<ArgumentNullException>((Action) act);
    }
    
    [Fact]
    public void ItShouldNotThrowIfNotNull()
    {
        // Arrange
        object? value = new object();

        // Act
        void act() => Guard.ThrowIfNull(value);

        // Assert does not throw
        act();
    }
    
    [Fact]
    public void ItShouldThrowIfNullOrWhitespace()
    {
        // Arrange
        string? value = null;

        // Act
        void act() => Guard.ThrowIfNullOrWhiteSpace(value);

        // Assert
        Assert.Throws<ArgumentNullException>((Action) act);
    }
    
    [Fact]
    public void ItShouldNotThrowIfNotNullOrWhitespace()
    {
        // Arrange
        string? value = "Test";

        // Act
        void act() => Guard.ThrowIfNullOrWhiteSpace(value);

        // Assert does not throw
        act();
    }
}