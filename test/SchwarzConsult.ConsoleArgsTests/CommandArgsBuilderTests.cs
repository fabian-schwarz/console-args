using System;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class CommandArgsBuilderTests
{
    [Fact]
    public void ItShouldBuild()
    {
        // Arrange
        var builder = new CommandArgsBuilder();

        // Act
        builder.AddCommand();
        builder.AddCommand();
        builder.AddCommand();
        builder.AddGlobalArgument("test1", "test1");
        builder.AddGlobalArgument("test2", "test2");
        builder.AddGlobalArgument("test3", "test3");
        var built = builder.Build();

        // Assert
        Assert.Equal(3, built.Commands.Count);
        Assert.Equal(3, built.GlobalArguments.Count);
    }

    [Fact]
    public void ItShouldAddGlobalArgument1()
    {
        // Arrange
        var builder = new CommandArgsBuilder();
        
        // Act
        builder.AddGlobalArgument("test", "description", Validate.AsInt);
        var built = builder.Build();

        // Assert
        Assert.Single(built.GlobalArguments);
        Assert.Equal("test", built.GlobalArguments[0].Name);
        Assert.Equal("description", built.GlobalArguments[0].Description);
        Assert.Equal(string.Empty, built.GlobalArguments[0].Abbreviation);
        Assert.NotNull(built.GlobalArguments[0].Validator);
        Assert.False(built.GlobalArguments[0].IsRequired);
        Assert.False(built.GlobalArguments[0].IsSwitch);
    }
    
    [Fact]
    public void ItShouldAddGlobalArgument2()
    {
        // Arrange
        var builder = new CommandArgsBuilder();
        
        // Act
        builder.AddGlobalArgument("test", "abbreviation", "description", Validate.AsInt);
        var built = builder.Build();

        // Assert
        Assert.Single(built.GlobalArguments);
        Assert.Equal("test", built.GlobalArguments[0].Name);
        Assert.Equal("description", built.GlobalArguments[0].Description);
        Assert.Equal("abbreviation", built.GlobalArguments[0].Abbreviation);
        Assert.NotNull(built.GlobalArguments[0].Validator);
        Assert.False(built.GlobalArguments[0].IsRequired);
        Assert.False(built.GlobalArguments[0].IsSwitch);
    }
    
    [Fact]
    public void ItShouldAddGlobalArgument3()
    {
        // Arrange
        var builder = new CommandArgsBuilder();
        
        // Act
        builder.AddGlobalArgument(new ArgumentKeys("test", "abbreviation"), "description", Validate.AsInt);
        var built = builder.Build();

        // Assert
        Assert.Single(built.GlobalArguments);
        Assert.Equal("test", built.GlobalArguments[0].Name);
        Assert.Equal("description", built.GlobalArguments[0].Description);
        Assert.Equal("abbreviation", built.GlobalArguments[0].Abbreviation);
        Assert.NotNull(built.GlobalArguments[0].Validator);
        Assert.False(built.GlobalArguments[0].IsRequired);
        Assert.False(built.GlobalArguments[0].IsSwitch);
    }
    
    [Fact]
    public void ItShouldAddGlobalArgument4()
    {
        // Arrange
        var builder = new CommandArgsBuilder();
        
        // Act
        builder.AddGlobalArgument("test", "description");
        var built = builder.Build();

        // Assert
        Assert.Single(built.GlobalArguments);
        Assert.Equal("test", built.GlobalArguments[0].Name);
        Assert.Equal("description", built.GlobalArguments[0].Description);
        Assert.Equal(string.Empty, built.GlobalArguments[0].Abbreviation);
        Assert.Null(built.GlobalArguments[0].Validator);
        Assert.False(built.GlobalArguments[0].IsRequired);
        Assert.False(built.GlobalArguments[0].IsSwitch);
    }

    [Fact]
    public void ItShouldAddDefaultHelp()
    {
        // Arrange
        var builder = new CommandArgsBuilder();
        
        // Act
        builder.AddDefaultHelp(false, "asd", "dsa");
        var built = builder.Build();

        // Assert
        Assert.False(built.DefaultHelp.IsEnabled);
        Assert.Equal("asd", built.DefaultHelp.Name);
        Assert.Equal("dsa", built.DefaultHelp.Abbreviation);
    }
}