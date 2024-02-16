using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;

namespace MauiAspire.App;

internal class MauiHostEnvironment : IHostEnvironment
{
    public string ApplicationName { get; set; } = typeof(MauiHostEnvironment).Assembly.GetName().Name!;

    public IFileProvider ContentRootFileProvider { get; set; } = new PhysicalFileProvider(AppContext.BaseDirectory);

    public string ContentRootPath { get; set; } = AppContext.BaseDirectory;

    public string EnvironmentName { get; set; } = "Development"; // TODO: Get the correct EnvironmentName
}
