# .NET MAUI + .NET Aspire - Better Together

[![NuGet package](https://img.shields.io/nuget/v/AspireMobile.svg)](https://nuget.org/packages/MauiAspire)

| Package | Version |
| ------- | ------- |
| AspireMobile.Hosting | [![NuGet Version](https://img.shields.io/nuget/v/AspireMobile.Hosting.svg)](https://nuget.org/packages/AspireMobile.Hosting) |
| AspireMobile.SettingsGenerator | [![NuGet Version](https://img.shields.io/nuget/v/AspireMobile.SettingsGenerator.svg)](https://nuget.org/packages/AspireMobile.SettingsGenerator) |
| MauiAspire.App | [![NuGet Version](https://img.shields.io/nuget/v/MauiAspire.App.svg)](https://nuget.org/packages/MauiAspire.App) |
| MauiAspire.ProjectTemplates | [![NuGet Version](https://img.shields.io/nuget/v/MauiAspire.ProjectTemplates.svg)](https://nuget.org/packages/MauiAspire.ProjectTemplates) |


This project enables .NET MAUI and other mobile clients to work .NET Aspire. It also includes a .NET MAUI + Aspire project template.

## Status

We're taking a staged approach to adding Aspire support to .NET MAUI / mobile clients:

**Experimental stage:** Support Aspire without any product changes to MAUI itself, Aspire, or Visual Studio. That's this project. It works and you can use it today. But there are rough edges, where the experience would be better making product changes. One main goal of the
this project to get feedback using it - from you - to help shape the product changes.

**Preview / release stages:** This is when we provide official support. This repo will go away, replaced with code in the
[dotnet/maui](https://github.com/dotnet/maui), [dotnet/aspire](https://github.com/dotnet/aspire), and other repos. Some package names may change.

Terminology note: The packages here with `Mobile` in the name help support Aspire with any mobile client (e.g., .NET Android, .NET iOS, Uno, Avalonia). The packages with `Maui` in the name are MAUI specific functionality.

## Sharing feedback

To report bugs with this code, create an issue on the [Issues](https://github.com/BretJohnson/aspire-mobile/issues) tab. PRs are also welcome.

To share feedback on what you'd like to see for the productized experience, use the [Provide good support for .NET Aspire with MAUI](https://github.com/dotnet/maui/discussions/21064) discussion topic in the MAUI repo.

## Benefits

[.NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/get-started/aspire-overview) when combined with .NET MAUI or other .NET UI frameworks, makes it easier to build cloud native client apps.
It brings several benefits:

- App support for the three pillars of observability: logs, metrics, and traces. This data shows on the Aspire dashboard for dev scenarios. For production, it can be exported to
OpenTelemetry compatible destinations like App Insights.
- [Distributed tracing](https://learn.microsoft.com/en-us/dotnet/core/diagnostics/distributed-tracing): See what happens end-to-end for a particular request, as it starts on the client and traverses multiple cloud services.
- Endpoint configuration. Aspire tells the app where to find its endpoints (e.g. URL for REST API service), whether they are running locally on the developer desktop or in
the cloud.
- Easier addition of cloud services to your app: There are large & growing set of Aspire components for particular cloud services.

## Creating a new MAUI+Aspire project

Install the templates:
```
dotnet new install MauiAspire.ProjectTemplates
```

In Visual Studio: `File` / `New Project` / `.NET MAUI App with Aspire` / `MyApp`

Or create a project from the CLI:
```
dotnet new maui-aspire -o MyApp
```

To also include a sample web client along with the MAUI client:
```
dotnet new maui-aspire -o MyApp --IncludeWeb true
```

## Usage

Launch `MyApp.AppHost` project to start any Aspire managed servies and the Aspire dashboard. You probably will want to launch without debugging (Ctrl-F5).

Launching AppHost the first time will also generate the `AspireAppSettings.g.cs` file in the MauiApp source directory.
Normally Aspire passess configuration settings as environment variables when launching services/clients, but for MAUI
apps (at least today) those settings are instead generated here and included in the app at build time.

Once AppHost is running, then switch the startup project to the `MyApp` project to run the MAUI app itself (debug or non-debug).
Hit the Load Weather button and you should see the MAUI app fetch weather data from the minimal API service running on your desktop, with activity across all services and the MAUI client itself tracked in the Aspire dashboard.

## Adding Aspire support to an existing MAUI project

Create a new solution from the template with the same name as your existing app (`MyCoolApp` in the example below). We'll copy from it.
```
dotnet new install MauiAspire.ProjectTemplates
dotnet new maui-aspire -o MyCoolApp
```

Copy the `MyCoolApp.ClientStub` project to your app solution. It doesn't need any changes.

Copy the `MyCoolApp.AppHost` project to your app solution. Assuming you don't you don't want to
use the REST simple API service included in the template, make these changes:
* `Program.cs`: remove mention of `apiService`
* `MyCoolApp.AppHost.csproj`: remove the `PackageReference` to the `MyCoolApiService`.

For the `MyCoolApp` MAUI project created by the template:
* `AppDefaultsExtensions.cs`: copy this file to your app. It doesn't require any changes, but you can customize it if you wish. See [.NET Aspire service defaults](https://learn.microsoft.com/en-us/dotnet/aspire/fundamentals/service-defaults) for customization.
* `MauiProgram.cs`: copy the lines from the template that configure the MauiAppBuilder

## Using the latest main CI builds

To use the latest CI built packages (as opposed to those published to nuget.org), create a nuget.config file with contents below. Place it in a parent directory to the directory you use for testing.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <packageSources>
    <add key="aspire-mobile" value="https://pkgs.dev.azure.com/bretjohn-public/aspire-mobile/_packaging/aspire-mobile/nuget/v3/index.json" />
  </packageSources>
</configuration>
```
