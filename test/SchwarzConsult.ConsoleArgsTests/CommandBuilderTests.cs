using System;
using System.Threading.Tasks;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class CommandBuilderTests
{
    [Fact]
    public void ItShouldBuild()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.SetVerb("Verb");
        builder.SetDescription("Description");
        builder.AddArgument("Argument1", "Description1");
        builder.AddArgument("Argument2", "Description2");
        builder.SetHandler<TestHandler1>();
        builder.AddSubCommand()
            .SetVerb("VerbSub1")
            .SetDescription("SubDescription1")
            .AddArgument("SubArgument1", "SubDescription1")
            .AddArgument("SubArgument2", "SubDescription2")
            .SetHandler<TestHandler2>()
            .Done()
        .AddSubCommand()
            .SetVerb("VerbSub2")
            .SetDescription("SubDescription2")
            .AddArgument("SubArgument3", "SubDescription1")
            .AddArgument("SubArgument4", "SubDescription2")
            .SetHandler<TestHandler3>()
            .Done();

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.Equal("Verb", command.Verb);
        Assert.Equal("Description", command.Description);
        Assert.NotNull(command.Arguments);
        Assert.Equal(2, command.Arguments!.Count);
        Assert.Equal("Argument1", command.Arguments[0].Name);
        Assert.Equal("Argument2", command.Arguments[1].Name);
        Assert.Equal(typeof(TestHandler1), command.Handler);
        
        Assert.NotNull(command.SubCommands);
        Assert.Equal(2, command.SubCommands!.Count);
        
        Assert.Equal("VerbSub1", command.SubCommands[0].Verb);
        Assert.Equal("SubDescription1", command.SubCommands[0].Description);
        Assert.Equal(2, command.SubCommands[0].Arguments!.Count);
        Assert.Equal("SubArgument1", command.SubCommands[0].Arguments![0].Name);
        Assert.Equal("SubArgument2", command.SubCommands[0].Arguments![1].Name);
        Assert.Equal(typeof(TestHandler2), command.SubCommands[0].Handler);
        
        Assert.Equal("VerbSub2", command.SubCommands[1].Verb);
        Assert.Equal("SubDescription2", command.SubCommands[1].Description);
        Assert.Equal(2, command.SubCommands[1].Arguments!.Count);
        Assert.Equal("SubArgument3", command.SubCommands[1].Arguments![0].Name);
        Assert.Equal("SubArgument4", command.SubCommands[1].Arguments![1].Name);
        Assert.Equal(typeof(TestHandler3), command.SubCommands[1].Handler);
    }

    [Fact]
    public void ItShouldThrowOnDoneWithoutParent()
    {
        // Arrange
        var builder = new CommandBuilder(null);

        // Act
        ICommandBuilder Act() => builder.Done();

        // Assert
        Assert.Throws<ArgumentException>(Act);
    }
    
    [Fact]
    public void ItShouldAddToParentOnDone()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddSubCommand()
            .SetVerb("Test")
            .Done();
        

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.SubCommands);
        Assert.Single(command.SubCommands!);
        Assert.Equal("Test", command.SubCommands![0].Verb);
    }
    
    [Fact]
    public void ItShouldSetHandler()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.SetHandler<TestHandler1>();

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Handler);
        Assert.Equal(typeof(TestHandler1), command.Handler);
    }
    
    [Fact]
    public void ItShouldSetVerb()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.SetVerb("Test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.Equal("Test", command.Verb);
    }
    
    [Fact]
    public void ItShouldSetDescription()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.SetDescription("Test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.Equal("Test", command.Description);
    }

    [Fact]
    public void ItShouldAddRequiredArgument1()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddRequiredArgument(new ArgumentKeys("test", "test"), "test", 
            _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddRequiredArgument2()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddRequiredArgument("test", "test", "test", 
            _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddRequiredArgument3()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddRequiredArgument(new ArgumentKeys("test", "test"), "test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.Null(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddRequiredArgument4()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddRequiredArgument("test", "test", "test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.Null(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddOptionalArgument1()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddOptionalArgument(new ArgumentKeys("test", "test"), "test", _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddOptionalArgument2()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddOptionalArgument("test", "test", "test", _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddOptionalArgument3()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddOptionalArgument("test", "test", _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal(string.Empty, command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddOptionalArgument4()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddOptionalArgument("test", "test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal(string.Empty, command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.Null(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddArgument1()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddArgument(new ArgumentKeys("test", "test"), "test", true, _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddArgument2()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddArgument("test", "test", "test", true, _ => Task.FromResult(true));

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.True(command.Arguments[0].IsRequired);
        Assert.False(command.Arguments[0].IsSwitch);
        Assert.NotNull(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddSwitchArgument1()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddSwitchArgument(new ArgumentKeys("test", "test"), "test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.True(command.Arguments[0].IsSwitch);
        Assert.Null(command.Arguments[0].Validator);
    }
    
    [Fact]
    public void ItShouldAddSwitchArgument2()
    {
        // Arrange
        var builder = new CommandBuilder(null);
        builder.AddSwitchArgument("test", "test", "test");

        // Act
        var command = builder.Build();

        // Assert
        Assert.NotNull(command);
        Assert.NotNull(command.Arguments);
        Assert.Single(command.Arguments!);
        Assert.Equal("test", command.Arguments![0].Name);
        Assert.Equal("test", command.Arguments[0].Abbreviation);
        Assert.Equal("test", command.Arguments[0].Description);
        Assert.False(command.Arguments[0].IsRequired);
        Assert.True(command.Arguments[0].IsSwitch);
        Assert.Null(command.Arguments[0].Validator);
    }
    
    private class TestHandler1 : ICommandHandler
    {
        public Task Handle(ICommandArgumentsBag argumentsBag)
        {
            throw new NotImplementedException();
        }
    }
    
    private class TestHandler2 : ICommandHandler
    {
        public Task Handle(ICommandArgumentsBag argumentsBag)
        {
            throw new NotImplementedException();
        }
    }
    
    private class TestHandler3 : ICommandHandler
    {
        public Task Handle(ICommandArgumentsBag argumentsBag)
        {
            throw new NotImplementedException();
        }
    }
}