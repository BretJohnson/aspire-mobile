using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AspireStarterApplication._1;

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

//-:cnd:noEmit
#if DEBUG
        mauiAppBuilder.Configuration.AddInMemoryCollection(AspireAppSettings.Settings);
#endif
//+:cnd:noEmit

        mauiAppBuilder.AddAppDefaults();

//-:cnd:noEmit
#if DEBUG
        mauiAppBuilder.Logging.AddDebug();
#endif
//+:cnd:noEmit

        builder.Services.AddHttpClient<WeatherApiClient>(client =>
        {
            // This URL uses "https+http://" to indicate HTTPS is preferred over HTTP.
            // Learn more about service discovery scheme resolution at https://aka.ms/dotnet/sdschemes.
            client.BaseAddress = new("https+http://apiservice");
        });

        mauiAppBuilder.Services.AddSingleton<MainPage>();

        MauiApp mauiApp = mauiAppBuilder.Build();
        mauiApp.InitOpenTelemetryServices();
        return mauiApp;
    }
}
