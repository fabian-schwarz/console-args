using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class CommandServiceTests
{
    [Fact]
    public void ItShouldThrowIfCantFindCommand()
    {
        // Arrange
        var args = new[] {"aks", "app", "down", "--acr", "asd", "-p", "123"};
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "aks",
                SubCommands = new List<Command>
                {
                    new()
                    {
                        Verb = "app",
                        SubCommands = new List<Command>
                        {
                            new() {Verb = "up"}
                        }
                    }
                }
            }
        };
        var commandService = new CommandService();
        
        // Act
        void Act() => commandService.ExtractCommandHierarchy(commands, args);
        
        // Assert
        Assert.Throws<ConsoleAppException>(Act);
    }
    
    [Fact]
    public void ItShouldReturnHierarchy()
    {
        // Arrange
        var args = new[] {"aks", "app", "up", "--acr", "asd", "-p", "123"};
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "aks",
                SubCommands = new List<Command>
                {
                    new()
                    {
                        Verb = "app",
                        SubCommands = new List<Command>
                        {
                            new() {Verb = "up"}
                        }
                    }
                }
            }
        };
        var commandService = new CommandService();
        
        // Act
        var result = commandService.ExtractCommandHierarchy(commands, args);
        
        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);
        Assert.Equal("aks", result[0].Verb);
        Assert.Equal("app", result[1].Verb);
        Assert.Equal("up", result[2].Verb);
    }

    [Fact]
    public void ItShouldExtractArgumentValuesForCommand()
    {
        // Arrange
        var args = new[] {"test", "--location", "westeurope", "-n", "test", "--tags", "test", "--debug", "true"};
        var command = new Command
        {
            Verb = "test",
            Arguments = new List<Argument>
            {
                new()
                {
                    Abbreviation = "l",
                    Name = "location",
                    IsRequired = true
                },
                new()
                {
                    Abbreviation = string.Empty,
                    Name = "managed-by",
                    IsRequired = false
                },
                new()
                {
                    Abbreviation = "n",
                    Name = "Name",
                    IsRequired = true
                }
            }
        };
        var globalArgs = new List<Argument>
        {
            new() {Name = "debug"},
            new() {Name = "query"},
        };
        var commandService = new CommandService();

        // Act
        var result = commandService.ExtractArgumentValuesForCommand(globalArgs, command, args);

        // Assert
        Assert.NotNull(result);
        var found = result.TryGetValueByAbbreviation("l", out var value);
        Assert.True(found);
        Assert.Equal("westeurope", value);
        found = result.TryGetValueByAbbreviation("n", out value);
        Assert.True(found);
        Assert.Equal("test", value);
        found = result.TryGetValueByName("debug", out value);
        Assert.True(found);
        Assert.Equal("true", value);
    }

    [Fact]
    public void ItShouldRegisterCommandHandlerTypesInServices()
    {
        // Arrange
        var services = new ServiceCollection();
        var commands = new List<Command>
        {
            new()
            {
                Verb = "test",
                Handler = typeof(TestHandler1)
            },
            new()
            {
                Verb = "test3",
            },
            new()
            {
                Verb = "test3",
                Handler = typeof(TestHandler2)
            }
        };
        var commandService = new CommandService();
        
        // Act
        commandService.RegisterCommandHandlers(commands, services);
        var provider = services.BuildServiceProvider();
        
        // Assert
        var service = provider.GetService(typeof(TestHandler1));
        Assert.NotNull(service);
        Assert.True(service is TestHandler1);
        service = provider.GetService(typeof(TestHandler2));
        Assert.NotNull(service);
        Assert.True(service is TestHandler2);
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
}