# .NET MAUI + Aspire

***.NET MAUI support for Aspire***

[![NuGet package](https://img.shields.io/nuget/v/MauiAspire.svg)](https://nuget.org/packages/MauiAspire)

This project provides the integration code necessary to use .NET MAUI with Aspire. It also includes a .NET MAUI + Aspire project template. It's currently experimental, and we want
your feedback.


## Creating a new MAUI+Aspire project

**_Current status (to set expectations):_**
The NuGets aren't published quite yet and we're still fixing some things.

[Optional] To use the latest CI built packages create a nuget.config file with contents below. Put it in a parent directory to what you use for testing.

```
<?xml version="1.0" encoding="utf-8"?>
<configuration>
<packageSources>
    <add key="aspire-mobile" value="https://pkgs.dev.azure.com/bretjohn-public/aspire-mobile/_packaging/aspire-mobile/nuget/v3/index.json" />
</packageSources>
</configuration>
```

Install the templates:
```
dotnet new install MauiAspire.ProjectTemplates
```

In Visual Studio: `File` / `New Project` / `.NET MAUI App with Aspire` / `MyApp`

or create a project from the CLI:
```
dotnet new maui-aspire -o MyApp
```

## Usage

Launch `MyApp.AppHost` project to start any Aspire managed servies and the
Aspire dashboard.

Launching AppHost the first time will also generate the `AspireAppSettings.g.cs` file in the MauiApp source directory.
Normally Aspire passess configuration settings as environment variables when launching services/clients, but for MAUI
apps (at least today) those settings are instead generated here and included in the app at build time.

Once AppHost is running, then launch the `MyApp` project to run the MAUI app itself. Hit the Load Weather button and
you should see the MAUI app fetch weather data from the minimal API service running on your desktop, with activity across
all services and the MAUI client itself tracked in the Aspire dashboard.
