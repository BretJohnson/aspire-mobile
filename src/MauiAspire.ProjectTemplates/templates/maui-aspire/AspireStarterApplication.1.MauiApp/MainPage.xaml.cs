namespace ClientAppsIntegration.MAUI
{
    public partial class MainPage : ContentPage
    {
        readonly WeatherApiClient _weatherApiClient;
        readonly CancellationTokenSource _closingCts = new();

        public MainPage(WeatherApiClient weatherApiClient)
        {
            InitializeComponent();

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

                var weather = await _weatherApiClient.GetWeatherAsync(_closingCts.Token);
                dgWeather.ItemsSource = weather;
                dgWeather.IsVisible = true;
            }
            catch (TaskCanceledException)
            {
                return;
            }
            catch (Exception ex)
            {
                dgWeather.IsVisible = false;
                dgWeather.ItemsSource = null;

                await DisplayAlert(ex.Message, "Error", "Ok");
            }

            pbLoading.IsVisible = false;
            btnLoad.IsEnabled = true;
        }
    }
}
