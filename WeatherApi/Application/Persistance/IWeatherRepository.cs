namespace WeatherApi.Interfaces;

public interface IWeatherRepository
{
    Task<Models.WeatherCurrent?> GetCurrentWeatherAsync(int cityId);
    Task<IEnumerable<Models.WeatherHistory>> GetWeatherHistoryAsync(int cityId, int days);

}