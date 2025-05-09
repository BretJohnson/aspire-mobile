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

        var scheme = mauiAppBuilder.Configuration.IsDevelopment() ? "http" : "https";
        mauiAppBuilder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new($"{scheme}://apiservice"));
        mauiAppBuilder.Services.AddSingleton<MainPage>();

        return mauiAppBuilder.Build();
    }
}
