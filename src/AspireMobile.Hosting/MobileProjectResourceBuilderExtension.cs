﻿using Aspire.Hosting.ApplicationModel;

namespace Aspire.Hosting;

public static class MobileProjectResourceBuilderExtension
{
    /// <summary>
    /// Adds a .NET mobile project (for example, a .NET MAUI project) to the application model. These projects are
    /// different than other Aspire managed projects. They aren't launched by Aspire itself, so Aspire can't pass
    /// settings to the app via environment variables. Instead those environment settings are written to a config
    /// file (AspireAppSettings.g.cs by default) which is included in the app at build time.
    /// </summary>
    /// <param name="builder">The <see cref="IDistributedApplicationBuilder"/>.</param>
    /// <param name="name">The name of the resource. This name shows in the dashboard and can be used for service discovery when referenced in a dependency.</param>
    /// <param name="projectDirectory">The path to the project file.</param>
    /// <returns>A reference to the <see cref="IResourceBuilder{ProjectResource}"/>.</returns>  
    public static IResourceBuilder<ProjectResource> AddMobileProject(this IDistributedApplicationBuilder builder, string name, string projectDirectory,
        string settingsFileName = "AspireAppSettings.g.cs", string clientStubProjectPath = "")
    {
        if (string.IsNullOrEmpty(clientStubProjectPath))
        {
            string projectName = Path.GetFileName(projectDirectory);
            clientStubProjectPath = Path.Combine(projectDirectory + ".ClientStub", projectName + ".ClientStub.csproj");

            if (! File.Exists(clientStubProjectPath))
            {
                throw new InvalidOperationException($"Default ClientStub project '{clientStubProjectPath}' doesn't exist. Specify a value for the optional clientStubProjectPath parameter if the path differs from the default.");
            }
        }

        string settingsPath = NormalizePathForCurrentPlatform(Path.Combine(builder.AppHostDirectory, projectDirectory, settingsFileName));

        return builder.AddProject(name, clientStubProjectPath).WithEnvironment(delegate (EnvironmentCallbackContext context)
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
