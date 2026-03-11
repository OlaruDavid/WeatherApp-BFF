namespace WeatherApi.Interfaces;

public interface ICityService
{
    Task<IEnumerable<DTOs.CityDto>> GetMainCitiesAsync(int skip, int take);
    Task<DTOs.WeatherDetailDto?> GetCityWeatherAsync(string cityName);
    Task<IEnumerable<string>> SearchCitiesAsync(string query);
}