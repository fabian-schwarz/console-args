using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

// ReSharper disable once CheckNamespace
namespace System;

/// <summary>
/// A configuration for a console application.
/// </summary>
public interface IConsoleAppConfiguration
{
    /// <summary>
    /// Gets or sets the configuration, build by using an appsettings.json file, environment variables and user secrets.
    /// </summary>
    IConfiguration Configuration { get; set; }
    /// <summary>
    /// Registers custom services in the DI container. Command line handler implementing <see cref="ICommandHandler"/> are registered automatically.
    /// </summary>
    /// <param name="services">Service collection to register services on.</param>
    /// <returns>Service collection.</returns>
    IServiceCollection ConfigureServices(IServiceCollection services);
    /// <summary>
    /// Configures the command line application arguments.
    /// </summary>
    /// <param name="app">Builder to configure the commands and arguments.</param>
    /// <returns>Command line application commands and arguments builder.</returns>
    ICommandArgsBuilder ConfigureCommands(ICommandArgsBuilder app);
}