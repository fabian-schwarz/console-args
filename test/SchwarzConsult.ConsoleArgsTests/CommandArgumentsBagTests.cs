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
}