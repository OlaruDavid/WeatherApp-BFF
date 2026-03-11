using System.Text.Json;
using WeatherApi.DTOs;
using WeatherApi.Interfaces;

namespace WeatherApi.Services;

public class WeatherService : IWeatherService
{
    private readonly ICityRepository _cityRepository;
    private readonly IWeatherRepository _weatherRepository;

    public WeatherService(ICityRepository cityRepository, IWeatherRepository weatherRepository)
    {
        _cityRepository = cityRepository;
        _weatherRepository = weatherRepository;
    }

    public async Task<IEnumerable<WeatherHistoryDto>> GetWeatherHistoryAsync(string cityName, int days)
    {
        var city = await _cityRepository.FindByNameAsync(cityName);
        if (city == null)
            return Enumerable.Empty<WeatherHistoryDto>();

        var targetCityId = (!city.IsMainCity && city.NearestMainCityId != null)
            ? city.NearestMainCityId.Value
            : city.Id;

        var history = await _weatherRepository.GetWeatherHistoryAsync(targetCityId, days);

        return history.Select(h =>
        {
            var json = JsonSerializer.Deserialize<JsonElement>(h.WeatherData);
            var main = json.GetProperty("main");

            return new WeatherHistoryDto
            {
                Temperature = main.GetProperty("temp").GetDouble(),
                Humidity = main.GetProperty("humidity").GetInt32(),
                WindSpeed = json.GetProperty("wind").GetProperty("speed").GetDouble(),
                Description = json.GetProperty("weather")[0].GetProperty("description").GetString() ?? "",
                RecordedAt = h.RecordedAt
            };
        });
    }
}