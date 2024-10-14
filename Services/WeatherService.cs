using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WeatherApp.Models;

namespace WeatherApp.Services
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<Weather> GetWeatherAsync(string city)
        {
            var apiKey = "d6637d4430ea87f88b23b0ea5b78afda";  // Use your OpenWeatherMap API key
            var response = await _httpClient.GetAsync($"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={apiKey}&units=metric");

            if (!response.IsSuccessStatusCode)
                return null;

            var responseBody = await response.Content.ReadAsStringAsync();
            var jsonDoc = JsonDocument.Parse(responseBody);

            var weather = new Weather
            {
                CityName = jsonDoc.RootElement.GetProperty("name").GetString(),
                Description = jsonDoc.RootElement.GetProperty("weather")[0].GetProperty("description").GetString(),
                Temperature = jsonDoc.RootElement.GetProperty("main").GetProperty("temp").GetSingle(),
                Humidity = jsonDoc.RootElement.GetProperty("main").GetProperty("humidity").GetSingle(),
                WindSpeed = jsonDoc.RootElement.GetProperty("wind").GetProperty("speed").GetSingle()
            };

            return weather;
        }
    }
}
