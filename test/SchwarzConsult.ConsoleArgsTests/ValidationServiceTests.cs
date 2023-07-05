using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SchwarzConsult.ConsoleArgs.Internal;
using Xunit;

namespace SchwarzConsult.ConsoleArgsTests;

public class ValidationServiceTests
{
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedVerbs()
    {
        // Arrange
        var commands = new List<Command>
        {
            new (){Verb = "duplicate"},
            new (){Verb = "test"},
            new (){Verb = "duplicate"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedVerbsRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new (){Verb = "test1"},
            new (){Verb = "test2"},
            new ()
            {
                Verb = "test3", 
                SubCommands = new List<Command>
                {
                    new (){Verb = "duplicate"},
                    new (){Verb = "test"},
                    new (){Verb = "duplicate"},
                }
            }
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnOkOnNoDuplicatedVerbsRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new (){Verb = "test1"},
            new (){Verb = "test2"},
            new ()
            {
                Verb = "test3", 
                SubCommands = new List<Command>
                {
                    new (){Verb = "test1"},
                    new (){Verb = "test2"},
                    new (){Verb = "test3"},
                }
            }
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedArgumentNames()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "duplicate"},
                    new (){Name = "test"},
                    new (){Name = "duplicate"},
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedArgumentNamesRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1"},
                    new (){Name = "test2"},
                    new (){Name = "test3"},
                },
                SubCommands = new List<Command>
                {
                    new ()
                    {
                        Verb = "test",
                        Arguments = new List<Argument>
                        {
                            new (){Name = "duplicate"},
                            new (){Name = "test"},
                            new (){Name = "duplicate"},
                        }, 
                    }
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueOnNoDuplicatedArgumentNamesRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1"},
                    new (){Name = "test2"},
                    new (){Name = "test3"},
                },
                SubCommands = new List<Command>
                {
                    new ()
                    {
                        Verb = "test",
                        Arguments = new List<Argument>
                        {
                            new (){Name = "test1"},
                            new (){Name = "test2"},
                            new (){Name = "test3"},
                        }, 
                    }
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedArgumentAbbreviations()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1", Abbreviation = "duplicate"},
                    new (){Name = "test2", Abbreviation = "test"},
                    new (){Name = "test3", Abbreviation = "duplicate"},
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedArgumentAbbreviationsRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1", Abbreviation = "test1"},
                    new (){Name = "test2", Abbreviation = "test2"},
                    new (){Name = "test3", Abbreviation = "test3"},
                },
                SubCommands = new List<Command>
                {
                    new ()
                    {
                        Verb = "test",
                        Arguments = new List<Argument>
                        {
                            new (){Name = "test1", Abbreviation = "duplicate"},
                            new (){Name = "test2", Abbreviation = "test"},
                            new (){Name = "test3", Abbreviation = "duplicate"},
                        }, 
                    }
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueOnNoDuplicatedArgumentAbbreviationsRecursive()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1", Abbreviation = "test1"},
                    new (){Name = "test2", Abbreviation = "test2"},
                    new (){Name = "test3", Abbreviation = "test3"},
                },
                SubCommands = new List<Command>
                {
                    new ()
                    {
                        Verb = "test",
                        Arguments = new List<Argument>
                        {
                            new (){Name = "test1", Abbreviation = "test1"},
                            new (){Name = "test2", Abbreviation = "test2"},
                            new (){Name = "test3", Abbreviation = "test3"},
                        }, 
                    }
                }
            },
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateDuplicationsRecursive(commands);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ItShouldReturnTrueWhenNoRequiredArgumentsExist()
    {
        // Arrange
        var command = new Command
        {
            Arguments = new List<Argument>
            {
                new (){Name = "test1", IsRequired = false},
                new (){Name = "test2", IsRequired = false},
                new (){Name = "test3", IsRequired = false}
            }
        };
        var bag = new CommandArgumentsBag();
        var service = new ValidationService();

        // Act
        var result = service.ValidateRequiredArgumentsSet(command, bag);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseWhenRequiredArgumentsNotExist()
    {
        // Arrange
        var command = new Command
        {
            Arguments = new List<Argument>
            {
                new (){Name = "test1", IsRequired = false},
                new (){Name = "test2", IsRequired = true},
                new (){Name = "test3", IsRequired = false}
            }
        };
        var bag = new CommandArgumentsBag();
        var service = new ValidationService();

        // Act
        var result = service.ValidateRequiredArgumentsSet(command, bag);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueWhenRequiredArgumentsExist()
    {
        // Arrange
        var command = new Command
        {
            Arguments = new List<Argument>
            {
                new (){Name = "test1", IsRequired = false},
                new (){Name = "test2", IsRequired = true},
                new (){Name = "test3", IsRequired = false}
            }
        };
        var bag = new CommandArgumentsBag();
        bag.Add("test2", string.Empty, "test");
        var service = new ValidationService();

        // Act
        var result = service.ValidateRequiredArgumentsSet(command, bag);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public async Task ItShouldReturnFalseWhenArgumentValidationFails()
    {
        // Arrange
        var command = new Command
        {
            Arguments = new List<Argument>
            {
                new (){Name = "test1", Validator = Validate.AsBoolean},
            }
        };
        var bag = new CommandArgumentsBag();
        bag.Add("test1", string.Empty, "test");
        var service = new ValidationService();
        
        // Act
        var result = await service.ValidateArgumentValues(command, bag);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public async Task ItShouldReturnTrueWhenArgumentValidationSucceeds()
    {
        // Arrange
        var command = new Command
        {
            Arguments = new List<Argument>
            {
                new (){Name = "test1", Validator = Validate.AsBoolean},
            }
        };
        var bag = new CommandArgumentsBag();
        bag.Add("test1", string.Empty, "true");
        var service = new ValidationService();
        
        // Act
        var result = await service.ValidateArgumentValues(command, bag);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public async Task ItShouldReturnTrueWhenNoArguments()
    {
        // Arrange
        var command = new Command();
        var bag = new CommandArgumentsBag();
        var service = new ValidationService();
        
        // Act
        var result = await service.ValidateArgumentValues(command, bag);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedGlobalArgumentAndCommandArgumentNames()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1"},
                    new (){Name = "test2"},
                    new (){Name = "duplicate"},
                }
            },
        };
        var globalArgs = new List<Argument>
        {
            new() {Name = "duplicate"},
            new() {Name = "test3"},
            new() {Name = "test4"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsDoNotOverlapRecursive(commands, globalArgs);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueOnNoDuplicatedGlobalArgumentAndCommandArgumentNames()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1"},
                    new (){Name = "test2"},
                    new (){Name = "test3"},
                }
            },
        };
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4"},
            new() {Name = "test5"},
            new() {Name = "test6"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsDoNotOverlapRecursive(commands, globalArgs);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnDuplicatedGlobalArgumentAndCommandArgumentAbbreviations()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1", Abbreviation = "test1"},
                    new (){Name = "test2", Abbreviation = "test2"},
                    new (){Name = "test3", Abbreviation = "duplicate"},
                }
            },
        };
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4", Abbreviation = "duplicate"},
            new() {Name = "test5", Abbreviation = "test5"},
            new() {Name = "test6", Abbreviation = "test6"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsDoNotOverlapRecursive(commands, globalArgs);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueOnNoDuplicatedGlobalArgumentAndCommandArgumentAbbreviations()
    {
        // Arrange
        var commands = new List<Command>
        {
            new ()
            {
                Verb = "test",
                Arguments = new List<Argument>
                {
                    new (){Name = "test1", Abbreviation = "test1"},
                    new (){Name = "test2", Abbreviation = "test2"},
                    new (){Name = "test3", Abbreviation = "test3"},
                }
            },
        };
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4", Abbreviation = "test4"},
            new() {Name = "test5", Abbreviation = "test5"},
            new() {Name = "test6", Abbreviation = "test6"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsDoNotOverlapRecursive(commands, globalArgs);

        // Assert
        Assert.True(result.IsValid);
    }

    [Fact]
    public void ItShouldReturnTrueOnGlobalArgsAbbreviationsUnique()
    {
        // Arrange
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4", Abbreviation = "test4"},
            new() {Name = "test5", Abbreviation = "test5"},
            new() {Name = "test6", Abbreviation = "test6"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsUnique(globalArgs);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnGlobalArgsAbbreviationsNotUnique()
    {
        // Arrange
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4", Abbreviation = "duplicate"},
            new() {Name = "test5", Abbreviation = "test5"},
            new() {Name = "test6", Abbreviation = "duplicate"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsUnique(globalArgs);

        // Assert
        Assert.False(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnTrueOnGlobalArgsNamesUnique()
    {
        // Arrange
        var globalArgs = new List<Argument>
        {
            new() {Name = "test4"},
            new() {Name = "test5"},
            new() {Name = "test6"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsUnique(globalArgs);

        // Assert
        Assert.True(result.IsValid);
    }
    
    [Fact]
    public void ItShouldReturnFalseOnGlobalArgsNamesNotUnique()
    {
        // Arrange
        var globalArgs = new List<Argument>
        {
            new() {Name = "duplicate"},
            new() {Name = "test5"},
            new() {Name = "duplicate"},
        };
        var service = new ValidationService();

        // Act
        var result = service.ValidateGlobalArgumentsUnique(globalArgs);

        // Assert
        Assert.False(result.IsValid);
    }
}