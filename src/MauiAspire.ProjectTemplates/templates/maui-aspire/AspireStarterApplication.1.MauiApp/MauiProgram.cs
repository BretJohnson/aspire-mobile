using AspireAppClientIntegration.MAUI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ClientAppsIntegration.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var mauiAppBuilder = MauiApp.CreateBuilder();

            mauiAppBuilder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var wrapperMauiAppBuilder = new WrapperMauiAppBuilder(mauiAppBuilder);
#if DEBUG
            wrapperMauiAppBuilder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif

            wrapperMauiAppBuilder.AddAppDefaults();

#if DEBUG
            wrapperMauiAppBuilder.Logging.AddDebug();
#endif

            var scheme = wrapperMauiAppBuilder.Environment.IsDevelopment() ? "http" : "https";
            wrapperMauiAppBuilder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new($"{scheme}://apiservice"));
            wrapperMauiAppBuilder.Services.AddSingleton<MainPage>();
            if (wrapperMauiAppBuilder.Environment.IsDevelopment())
            {
                wrapperMauiAppBuilder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
            }

            return wrapperMauiAppBuilder.Build();
        }
    }
}
