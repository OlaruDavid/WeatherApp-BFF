namespace WeatherApi.Interfaces;

public interface IWeatherService
{
    Task<IEnumerable<DTOs.WeatherHistoryDto>> GetWeatherHistoryAsync(string cityName, int days);
    
}