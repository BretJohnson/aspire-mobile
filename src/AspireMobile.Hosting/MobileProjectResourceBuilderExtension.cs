using System.Reflection;

namespace Aspire.Hosting;

public static class MobileProjectResourceBuilderExtension
{
    /// <summary>
    /// Adds a .NET project to the application model. 
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource. This name will be used for service discovery when referenced in a dependency.</param>
    /// <param name="projectDirectory">The path to the project file.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{ProjectResource}"/>.</returns>
    public static IResourceBuilder<ProjectResource> AddMobileProject(this IDistributedApplicationBuilder builder, string name, string projectDirectory,
        string settingsFileName = "AspireAppSettings.g.cs")
    {
        string settingsPath = NormalizePathForCurrentPlatform(Path.Combine(builder.AppHostDirectory, projectDirectory, settingsFileName));

        string? myDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (string.IsNullOrEmpty(myDirectory))
        {
            throw new InvalidOperationException("Error getting path for AspireMobile.Hostingd.dll assembly");
        }
        string settingsGeneratorProjectPath = Path.Combine(myDirectory, "AspireMobile.Hosting.SettingsGenerator/AspireMobile.Hosting.SettingsGenerator.csproj");

        return builder.AddProject(name, settingsGeneratorProjectPath)
           .WithEnvironment(context =>
           {
               context.EnvironmentVariables.Add("ASPIRE_SETTINGS_PATH", settingsPath);
           });
    }

    private static string NormalizePathForCurrentPlatform(string path)
    {
        if (string.IsNullOrEmpty(path))
        {
            return path;
        }

        // Fix slashes
        path = path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);

        return Path.GetFullPath(path);
    }
}
