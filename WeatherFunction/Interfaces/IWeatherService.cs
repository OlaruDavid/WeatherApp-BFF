namespace WeatherFunction.Interfaces;

public interface IWeatherService
{
    Task<string> GetCurrentWeatherAsync(string cityName);
    Task<IEnumerable<string>> GetWeatherHistoryAsync(string cityName, int days);
    Task<string> FetchAndSaveWeatherAsync(Models.City city);
}