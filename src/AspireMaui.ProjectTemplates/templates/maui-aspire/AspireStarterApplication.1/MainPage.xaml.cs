using Microsoft.Extensions.Logging;

namespace AspireStarterApplication._1;

public partial class MainPage : ContentPage
{
    readonly ILogger<MainPage> _logger;
    readonly WeatherApiClient _weatherApiClient;
    readonly CancellationTokenSource _closingCts = new();

    public MainPage(ILogger<MainPage> logger, WeatherApiClient weatherApiClient)
    {
        InitializeComponent();

        _logger = logger;
        _weatherApiClient = weatherApiClient;

        pbLoading.IsVisible = false;
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();

        _closingCts.Cancel();
    }

    async void OnButtonClick(object sender, EventArgs e)
    {
        btnLoad.IsEnabled = false;
        pbLoading.IsVisible = true;

        try
        {
            if (chkForceError.IsChecked == true)
            {
                throw new InvalidOperationException("Forced error!");
            }

            var weather = await _weatherApiClient.GetWeatherAsync(cancellationToken:_closingCts.Token);
            dgWeather.ItemsSource = weather;
            dgWeather.IsVisible = true;
        }
        catch (TaskCanceledException)
        {
            return;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading weather");

            dgWeather.IsVisible = false;
            dgWeather.ItemsSource = null;

            await DisplayAlert(ex.Message, "Error", "Ok");
        }

        pbLoading.IsVisible = false;
        btnLoad.IsEnabled = true;
    }
}
