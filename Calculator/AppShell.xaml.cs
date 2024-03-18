using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace Calculator
{
    public partial class AppShell : Shell
    {
        public const string ApiUrl = "http://api.openweathermap.org/data/2.5/weather?q=Eskilstuna,SE&units=metric&appid=e9e32f009ee59587e1eecd2e9fdf9346"; // min api 

        public AppShell()
        {
            InitializeComponent();
            WeatherAsync();
        }

        private async Task WeatherAsync() // hanterar vilken bild som ska visas vid under 0 grader och 0 gradeer eller över
        {
            try
            {
                var weatherData = await GetWeatherDataAsync();
                weatherIcon.Source = weatherData.Main.Temp >= 0 ? "sol.png" : "snö.png";
                temperatureLabel.Text = $"{weatherData.Main.Temp} °C";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching weather data: {ex.Message}");
            }
        }

        public async Task<WeatherData> GetWeatherDataAsync() //tar data från min api
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(ApiUrl);
                var weatherData = JsonConvert.DeserializeObject<WeatherData>(response);
                return weatherData;
            }
        }

        public class WeatherData
        {
            public MainInfo Main { get; set; }
        }

        public class MainInfo
        {
            public double Temp { get; set; }
        }
    }
}
