using System.Collections;

namespace Aspire.MAUIAppHost;

internal class Program
{
    public static void Main(string[] args)
    {
        IDictionary environmentVariables = Environment.GetEnvironmentVariables();

        string? settingsPath = (string?)environmentVariables["ASPIRE_SETTINGS_PATH"];
        if (settingsPath == null)
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
        var variablesToInclude = new HashSet<string> {
            "ASPNETCORE_ENVIRONMENT",
            "ASPNETCORE_URLS",
            "DOTNET_ENVIRONMENT",
            "DOTNET_LAUNCH_PROFILE",
            "DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION"
        };

        // Get the subset of variables that are provided by Aspire and sort them
        List<string> variableNames = new List<string>();
        foreach (object variableNameObject in environmentVariables.Keys)
        {
            string variableName = (string)variableNameObject;
            if (variablesToInclude.Contains(variableName) || variableName.StartsWith("OTEL_") || variableName.StartsWith("LOGGING__CONSOLE") || variableName.StartsWith("services__"))
            {
                variableNames.Add(variableName);
            }
        }
        variableNames.Sort();

        using (StreamWriter file = new StreamWriter(settingsPath))
        {
            file.Write("""
                    // This file is generated from the Aspire AppHost project. Re-run the Aspre AppHost
                    // to regenerate it.
                    
                    public static class AspireAppSettings
                    {
                        public static readonly Dictionary<string, string> Settings =
                            new Dictionary<string, string>
                            {

                    """);

            foreach (string variableName in variableNames)
            {
                string normalizedVariableName = variableName.Replace("__", ":");

                string valueLiteral = GetValueStringLiteral((string) environmentVariables[variableName]!);
                file.WriteLine($"""            ["{normalizedVariableName}"] = {valueLiteral},""");
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

