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
        var built = builder.Build();

        // Assert
        Assert.Equal(3, built.Count);
    }
}