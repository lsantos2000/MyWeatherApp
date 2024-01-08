// Presentation Layer: Program.cs in MyWeatherApp.ConsoleApp

using MyWeatherApp.Infrastructure;

namespace MyWeatherApp.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var weatherService = new WeatherService();

            try
            {
                string weather = await weatherService.GetWeatherAsync();
                Console.WriteLine(weather);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
