// This file is generated from the Aspire AppHost project. Re-run the Aspre AppHost
// to regenerate it.

public static class AspireAppSettings
{
    public static readonly Dictionary<string, string> Settings =
        new Dictionary<string, string>
        {
            ["DOTNET_SYSTEM_CONSOLE_ALLOW_ANSI_COLOR_REDIRECTION"] = "true",
            ["LOGGING:CONSOLE:FORMATTERNAME"] = "simple",
            ["LOGGING:CONSOLE:FORMATTEROPTIONS:TIMESTAMPFORMAT"] = "yyyy-MM-ddTHH:mm:ss.fffffff ",
            ["OTEL_BLRP_SCHEDULE_DELAY"] = "1000",
            ["OTEL_BSP_SCHEDULE_DELAY"] = "1000",
            ["OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EVENT_LOG_ATTRIBUTES"] = "true",
            ["OTEL_DOTNET_EXPERIMENTAL_OTLP_EMIT_EXCEPTION_LOG_ATTRIBUTES"] = "true",
            ["OTEL_EXPORTER_OTLP_ENDPOINT"] = "http://localhost:16222",
            ["OTEL_METRIC_EXPORT_INTERVAL"] = "1000",
            ["OTEL_RESOURCE_ATTRIBUTES"] = "service.instance.id=1067693a-ce1b-4fa0-8bb1-7f22f1678cd9",
            ["OTEL_SERVICE_NAME"] = "mauiclient",
            ["services:apiservice:0"] = "http://_http.localhost:5303",
            ["services:apiservice:1"] = "http://localhost:5303",
        };
}