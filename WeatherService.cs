// Infrastructure Layer: WeatherService.cs in MyWeatherApp.Infrastructure

using Newtonsoft.Json;
using MyWeatherApp.Domain;

namespace MyWeatherApp.Infrastructure
{
    public class WeatherService
    {
        private readonly HttpClient _httpClient;

        public WeatherService()
        {
            _httpClient = new HttpClient();
        }

        private async Task<string> GetCityByIPAsync()
        {
            var geoResponse = await _httpClient.GetAsync(Constants.IpApiUrl);
            if (!geoResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch geolocation data: {geoResponse.ReasonPhrase}");
            }

            var geoContent = await geoResponse.Content.ReadAsStringAsync();
            dynamic geoData = JsonConvert.DeserializeObject(geoContent);
            return geoData.city;
        }

        public async Task<string> GetWeatherAsync()
        {
            string city = await GetCityByIPAsync();
            if (string.IsNullOrWhiteSpace(city))
            {
                throw new InvalidOperationException("Unable to detect city from IP address.");
            }

            var weatherUrl = $"{Constants.WeatherstackApiUrl}?access_key={Constants.WeatherApiKey}&query={city}";
            var weatherResponse = await _httpClient.GetAsync(weatherUrl);

            if (!weatherResponse.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to fetch weather data: {weatherResponse.ReasonPhrase}");
            }

            var weatherContent = await weatherResponse.Content.ReadAsStringAsync();
            dynamic weatherData = JsonConvert.DeserializeObject(weatherContent);
            return $"Current temperature in {city} is {weatherData.current.temperature}°C";
        }
    }
}
