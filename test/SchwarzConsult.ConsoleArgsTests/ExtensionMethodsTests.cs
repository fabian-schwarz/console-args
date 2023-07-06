using System.Collections.Generic;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class ExtensionMethodsTests
{
    [Theory]
    [InlineData("existing1", true)]
    [InlineData("anotherExisting", true)]
    [InlineData("notThere", false)]
    [InlineData("shouldAlsoBeNotThere", false)]
    public void ItShouldFindArgumentsByName(string name, bool expected)
    {
        // Arrange
        var command = new Command
        {
            Verb = "test",
            Description = "description",
            Arguments = new List<Argument>
            {
                new() {Name = "blabla"},
                new() {Name = "existing1"},
                new() {Name = "dummy"},
                new() {Name = "anotherExisting"},
            },
            SubCommands = null,
            Handler = null
        };

        // Act
        var result = command.Arguments.FindArgumentByName(name);
        
        // Assert
        Assert.Equal(expected, result is not null);
    }
    
    [Theory]
    [InlineData("existing1", true)]
    [InlineData("anotherExisting", true)]
    [InlineData("notThere", false)]
    [InlineData("shouldAlsoBeNotThere", false)]
    public void ItShouldFindArgumentsByAbbreviation(string name, bool expected)
    {
        // Arrange
        var command = new Command
        {
            Verb = "test",
            Description = "description",
            Arguments = new List<Argument>
            {
                new() {Abbreviation = "blabla"},
                new() {Abbreviation = "existing1"},
                new() {Abbreviation = "dummy"},
                new() {Abbreviation = "anotherExisting"},
            },
            SubCommands = null,
            Handler = null
        };

        // Act
        var result = command.Arguments.FindArgumentByAbbreviation(name);
        
        // Assert
        Assert.Equal(expected, result is not null);
    }

    [Fact]
    public void ItShouldFindDuplicates()
    {
        // Arrange
        var arguments = new List<Argument>
        {
            new() {Name = "duplicate"},
            new() {Name = "test1"},
            new() {Name = "duplicate"},
            new() {Name = "test2"},
            new() {Name = "duplicate"},
        };

        // Act
        var result = arguments.FindDuplicates(a => a.Name);

        // Assert
        Assert.Single(result);
        Assert.Equal("duplicate", result[0]);
    }
    
    [Fact]
    public void ItShouldFindNoDuplicates()
    {
        // Arrange
        var arguments = new List<Argument>
        {
            new() {Name = "test1"},
            new() {Name = "test2"},
            new() {Name = "test3"},
            new() {Name = "test4"},
            new() {Name = "test5"},
        };

        // Act
        var result = arguments.FindDuplicates(a => a.Name);

        // Assert
        Assert.Empty(result);
    }
    
    [Fact]
    public void ItShouldFindDuplicatesAndRemoveEmptyEntries()
    {
        // Arrange
        var arguments = new List<Argument>
        {
            new() {Name = "duplicate"},
            new() {Name = string.Empty},
            new() {Name = "test1"},
            new() {Name = string.Empty},
            new() {Name = "duplicate"},
            new() {Name = string.Empty},
            new() {Name = "test2"},
            new() {Name = string.Empty},
            new() {Name = "duplicate"},
        };

        // Act
        var result = arguments.FindDuplicatesButRemoveEmptyStrings(a => a.Name ?? string.Empty);

        // Assert
        Assert.Single(result);
        Assert.Equal("duplicate", result[0]);
    }
    
    [Fact]
    public void ItShouldFindNoDuplicatesAndRemoveEmptyEntries()
    {
        // Arrange
        var arguments = new List<Argument>
        {
            new() {Name = "test1"},
            new() {Name = string.Empty},
            new() {Name = "test2"},
            new() {Name = string.Empty},
            new() {Name = "test3"},
            new() {Name = string.Empty},
            new() {Name = "test4"},
            new() {Name = string.Empty},
            new() {Name = "test5"},
        };

        // Act
        var result = arguments.FindDuplicatesButRemoveEmptyStrings(a => a.Name ?? string.Empty);

        // Assert
        Assert.Empty(result);
    }
}