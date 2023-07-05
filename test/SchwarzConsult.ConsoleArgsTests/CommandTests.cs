using System.Collections.Generic;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class CommandTests
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
        var result = command.FindArgumentByName(name);
        
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
        var result = command.FindArgumentByAbbreviation(name);
        
        // Assert
        Assert.Equal(expected, result is not null);
    }
}