using System;
using System.Linq;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class CommandArgumentsBagTests
{
    [Fact]
    public void ItShouldThrowIfArgumentExistsAlready()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        var value = new ArgumentValue("name", "abbreviation", "value");
        bag.Add(value);

        // Act
        void TestCode() => bag.Add(value);

        // Assert
        Assert.Throws<ConsoleAppException>(TestCode);
    }
    
    [Fact]
    public void ItShouldNotThrowIfArgumentNotAlreadyExists()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        var value = new ArgumentValue("name", "abbreviation", "value");
        bag.Add(value);

        // Act
        var exception = Record.Exception(() => bag.Add(new ArgumentValue("name1", "abbreviation1", "value")));

        // Assert does not throw
        Assert.Null(exception);
    }
    
    [Fact]
    public void ItShouldListAllArguments()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value"));

        // Act
        var actual = bag.List();

        // Assert does not throw
        Assert.Equal(3, actual.Count());
    }
    
    [Theory]
    [InlineData("name1", true, "value1")]
    [InlineData("name3", true, "value3")]
    [InlineData("notThere", false, null)]
    [InlineData("shouldAlsoBeNotThere", false, null)]
    public void ItShouldGetArgumentByName(string name, bool expectedExisting, string expectedValue)
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));

        // Act
        var actualExisting = bag.TryGetValueByName(name, out var actualValue);

        // Assert does not throw
        Assert.Equal(expectedExisting, actualExisting);
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData("abbreviation1", true, "value1")]
    [InlineData("abbreviation3", true, "value3")]
    [InlineData("notThere", false, null)]
    [InlineData("shouldAlsoBeNotThere", false, null)]
    public void ItShouldGetArgumentByAbbreviation(string abbreviation, bool expectedExisting, string expectedValue)
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));

        // Act
        var actualExisting = bag.TryGetValueByAbbreviation(abbreviation, out var actualValue);

        // Assert does not throw
        Assert.Equal(expectedExisting, actualExisting);
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData("name1", "abbreviation1", true, "value1")]
    [InlineData("name3", "abbreviation3", true, "value3")]
    [InlineData("notThere", "notThere", false, null)]
    [InlineData("shouldAlsoBeNotThere", "shouldAlsoBeNotThere", false, null)]
    public void ItShouldGetArgumentByAbbreviationOrName(string name, string abbreviation, bool expectedExisting, string expectedValue)
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));

        // Act
        var actualExisting = bag.TryGetValueByAbbreviationOrName(name, abbreviation, out var actualValue);

        // Assert does not throw
        Assert.Equal(expectedExisting, actualExisting);
        Assert.Equal(expectedValue, actualValue);
    }
    
    [Theory]
    [InlineData("name1", "abbreviation1", true, "value1")]
    [InlineData("name3", "abbreviation3", true, "value3")]
    [InlineData("notThere", "notThere", false, null)]
    [InlineData("shouldAlsoBeNotThere", "shouldAlsoBeNotThere", false, null)]
    public void ItShouldGetArgumentByKey(string name, string abbreviation, bool expectedExisting, string expectedValue)
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));

        // Act
        var actualExisting = bag.TryGetValue(new ArgumentKeys(name, abbreviation), out var actualValue);

        // Assert does not throw
        Assert.Equal(expectedExisting, actualExisting);
        Assert.Equal(expectedValue, actualValue);
    }

    [Fact]
    public void ItShouldTryGetValueByAbbreviationAs()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", id.ToString()));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        var value = bag.TryGetValueByAbbreviationAs("abbreviation2", Parse.AsGuid, out var actualValue);
        
        // Assert
        Assert.True(value);
        Assert.Equal(id, actualValue);
    }
    
    [Fact]
    public void ItShouldThrowIfNotTryGetValueByAbbreviationAs()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        void Act() => bag.TryGetValueByAbbreviationAs("abbreviation2", Parse.AsGuid, out _);
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    
    [Fact]
    public void ItShouldTryGetValueByNameAs()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", id.ToString()));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        var value = bag.TryGetValueByNameAs("name2", Parse.AsGuid, out var actualValue);
        
        // Assert
        Assert.True(value);
        Assert.Equal(id, actualValue);
    }
    
    [Fact]
    public void ItShouldThrowIfNotTryGetValueByNameAs()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        void Act() => bag.TryGetValueByNameAs("name2", Parse.AsGuid, out _);
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldTryGetValueByAbbreviationOrNameAs()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", id.ToString()));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        var value = bag.TryGetValueByAbbreviationOrNameAs("name2","abbreviation2", Parse.AsGuid, out var actualValue);
        
        // Assert
        Assert.True(value);
        Assert.Equal(id, actualValue);
    }
    
    [Fact]
    public void ItShouldThrowIfNotTryGetValueByAbbreviationOrNameAs()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        void Act() => bag.TryGetValueByAbbreviationOrNameAs("name2", "abbreviation2", Parse.AsGuid, out _);
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldTryGetValueByAs()
    {
        // Arrange
        var id = Guid.NewGuid();
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", id.ToString()));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        var value = bag.TryGetValueAs(new ArgumentKeys("name2","abbreviation2"), Parse.AsGuid, out var actualValue);
        
        // Assert
        Assert.True(value);
        Assert.Equal(id, actualValue);
    }
    
    [Fact]
    public void ItShouldThrowIfNotTryGetValueByAs()
    {
        // Arrange
        var bag = new CommandArgumentsBag();
        bag.Add(new ArgumentValue("name1", "abbreviation1", "value1"));
        bag.Add(new ArgumentValue("name2", "abbreviation2", "value2"));
        bag.Add(new ArgumentValue("name3", "abbreviation3", "value3"));
        
        // Act
        void Act() => bag.TryGetValueAs(new ArgumentKeys("name2", "abbreviation2"), Parse.AsGuid, out _);
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
}