using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.Hosting;

public static class ConfigurationExtensions
{
    /// <summary>
    /// Checks if the current host environment name is <see cref="Environments.Development"/>.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="ConfigurationManager"/>.</param>
    /// <returns>True if the environment name is <see cref="Environments.Development"/>, otherwise false.</returns>
    public static bool IsDevelopment(this ConfigurationManager configuration)
    {
        return configuration.IsEnvironment(Environments.Development);
    }

    /// <summary>
    /// Checks if the current host environment name is <see cref="Environments.Staging"/>.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="ConfigurationManager"/>.</param>
    /// <returns>True if the environment name is <see cref="Environments.Staging"/>, otherwise false.</returns>
    public static bool IsStaging(this ConfigurationManager configuration)
    {
        return configuration.IsEnvironment(Environments.Staging);
    }

    /// <summary>
    /// Checks if the current host environment name is <see cref="Environments.Production"/>.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="ConfigurationManager"/>.</param>
    /// <returns>True if the environment name is <see cref="Environments.Production"/>, otherwise false.</returns>
    public static bool IsProduction(this ConfigurationManager configuration)
    {
        return configuration.IsEnvironment(Environments.Production);
    }

    /// <summary>
    /// Compares the current host environment name against the specified value.
    /// </summary>
    /// <param name="configuration">An instance of <see cref="ConfigurationManager"/>.</param>
    /// <param name="environmentName">Environment name to validate against.</param>
    /// <returns>True if the specified name is the same as the current environment, otherwise false.</returns>
    public static bool IsEnvironment(
        this ConfigurationManager configuration,
        string environmentName)
    {
        string configEnvironment = configuration[HostDefaults.EnvironmentKey] ?? Environments.Production;

        configuration.AddEnvironmentVariables();

        return string.Equals(
            configEnvironment,
            environmentName,
            StringComparison.OrdinalIgnoreCase);
    }
}
