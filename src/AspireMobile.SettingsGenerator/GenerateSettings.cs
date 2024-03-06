// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.

using System.Collections;

namespace AspireMobile.SettingsGenerator;

public class GenerateSettings
{
    public static void Generate()    {
        IDictionary environmentVariables = Environment.GetEnvironmentVariables();

        string? settingsPath = (string?)environmentVariables["ASPIRE_SETTINGS_PATH"];
        if (settingsPath is null)
        {
            Console.Write("No ASPIRE_SETTINGS_PATH environment variable not found");
        }
        else
        {
            WriteSettings(environmentVariables, settingsPath);
            Console.Write($"Updated {settingsPath}");
        }

        Thread.Sleep(Timeout.Infinite);
    }

    static void WriteSettings(IDictionary environmentVariables, string settingsPath)
    {
        var variablesToInclude = new HashSet<string>
        {
            "ASPNETCORE_ENVIRONMENT",
            "ASPNETCORE_URLS",
            "DOTNET_ENVIRONMENT",
            "DOTNET_LAUNCH_PROFILE",
            "DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION"
        };

        var prefixesToRemove = new List<string>
        {
            "ASPNETCORE_",
            "DOTNET_",
        };

        // Get the subset of variables that are provided by Aspire and sort them
        List<string> variableNames = new List<string>();
        foreach (object variableNameObject in environmentVariables.Keys)
        {
            string variableName = (string)variableNameObject;
            if (variablesToInclude.Contains(variableName) || variableName.StartsWith("OTEL_") || variableName.StartsWith("LOGGING__CONSOLE") || variableName.StartsWith("services__"))
            {
                // Normalize the key, matching the logic here:
                // https://github.dev/dotnet/runtime/blob/main/src/libraries/Microsoft.Extensions.Configuration.EnvironmentVariables/src/EnvironmentVariablesConfigurationProvider.cs
                variableName = variableName.Replace("__", ":");

                // For defined prefixes, add the variable with the prefix removed, matching the logic
                // in EnvironmentVariablesConfigurationProvider.cs. Also add the variable with the
                // prefix intact, which matches the normal HostApplicationBuilder behavior, where
                // there's an EnvironmentVariablesConfigurationProvider added with and another one
                // without the prefix set.
                foreach (var prefix in prefixesToRemove)
                {
                    if (variableName.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                    {
                        variableNames.Add(variableName);
                        variableName = variableName.Substring(variableName.Length);
                        break;
                    }
                }

                variableNames.Add(variableName);
            }
        }

        variableNames.Sort();

        using (var file = new StreamWriter(settingsPath))
        {
            file.Write("""
                    // This file is generated from the Aspire AppHost project. Rerun the Aspire AppHost
                    // to regenerate it.
                    
                    public static class AspireAppSettings
                    {
                        public static readonly Dictionary<string, string> Settings =
                            new Dictionary<string, string>
                            {

                    """);

            foreach (string variableName in variableNames)
            {
                string valueLiteral = GetValueStringLiteral((string) environmentVariables[variableName]!);
                file.WriteLine($"""            ["{variableName}"] = {valueLiteral},""");
            }

            file.Write("""
                            };
                    }
                    """);
        }
    }

    static string GetValueStringLiteral(string value)
    {
        // If the string contains a quote or escape sequence, use a verbatim string literal, where "" is used for "
        if (value.Contains("\"") || value.Contains("\\"))
        {
            return "@\"" + value.Replace("\"", "\"\"") + "\"";
        }
        else return "\"" + value + "\"";
    }
}
