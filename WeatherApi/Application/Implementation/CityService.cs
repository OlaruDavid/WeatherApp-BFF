using System.Text.Json;
using WeatherApi.DTOs;
using WeatherApi.Interfaces;
using WeatherApi.Models;

namespace WeatherApi.Services;

public class CityService : ICityService
{
    private readonly ICityRepository _cityRepository;
    private readonly IWeatherRepository _weatherRepository;
    private readonly ISearchHistoryRepository _searchHistoryRepository;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly double _radiusKm;

    public CityService(
        ICityRepository cityRepository,
        IWeatherRepository weatherRepository,
        ISearchHistoryRepository searchHistoryRepository,
        IConfiguration configuration)
    {
        _cityRepository = cityRepository;
        _weatherRepository = weatherRepository;
        _searchHistoryRepository = searchHistoryRepository;
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenWeatherMapApiKey"] ?? throw new Exception("OpenWeatherMapApiKey lipseste!");
        _radiusKm = double.Parse(configuration["NearestCityRadiusKm"] ?? "50");
    }

    public async Task<IEnumerable<CityDto>> GetMainCitiesAsync(int skip, int take)
    {
        var cities = await _cityRepository.GetMainCitiesAsync(skip, take);
        var result = new List<CityDto>();

        foreach (var city in cities)
        {
            var weather = await _weatherRepository.GetCurrentWeatherAsync(city.Id);
            double temperature = 0;
            double humidity = 0;

            if (weather != null)
            {
                var json = JsonSerializer.Deserialize<JsonElement>(weather.WeatherData);
                temperature = json.GetProperty("main").GetProperty("temp").GetDouble();
                humidity = json.GetProperty("main").GetProperty("humidity").GetDouble();
            }

            result.Add(new CityDto
            {
                Id = city.Id,
                CityName = city.CityName,
                Latitude = city.Latitude,
                Longitude = city.Longitude,
                Temperature = temperature,
                Humidity = humidity
            });
        }

        return result;
    }

    public async Task<WeatherDetailDto?> GetCityWeatherAsync(string cityName)
    {
        var city = await _cityRepository.FindByNameAsync(cityName);

        if (city == null)
        {
            city = await GeocodeAndSaveCityAsync(cityName);
            if (city == null)
                return null;
        }

        await _searchHistoryRepository.AddSearchAsync(new SearchHistory
        {
            CityId = city.Id,
            CityName = city.CityName,
            SearchedAt = DateTime.UtcNow
        });

        var targetCityId = (!city.IsMainCity && city.NearestMainCityId != null)
            ? city.NearestMainCityId.Value
            : city.Id;

        var weather = await _weatherRepository.GetCurrentWeatherAsync(targetCityId);
        if (weather == null)
            return null;

        var json = JsonSerializer.Deserialize<JsonElement>(weather.WeatherData);
        var main = json.GetProperty("main");

        return new WeatherDetailDto
        {
            CityName = city.CityName,
            Latitude = city.Latitude,
            Longitude = city.Longitude,
            Temperature = main.GetProperty("temp").GetDouble(),
            FeelsLike = main.GetProperty("feels_like").GetDouble(),
            TempMin = main.GetProperty("temp_min").GetDouble(),
            TempMax = main.GetProperty("temp_max").GetDouble(),
            Humidity = main.GetProperty("humidity").GetInt32(),
            WindSpeed = json.GetProperty("wind").GetProperty("speed").GetDouble(),
            Description = json.GetProperty("weather")[0].GetProperty("description").GetString() ?? "",
            Icon = json.GetProperty("weather")[0].GetProperty("icon").GetString() ?? "",
            UpdatedAt = weather.UpdatedAt
        };
    }

    private async Task<City?> GeocodeAndSaveCityAsync(string cityName)
    {
        var geoUrl = $"https://api.openweathermap.org/geo/1.0/direct?q={cityName},RO&limit=1&appid={_apiKey}";
        var geoResponse = await _httpClient.GetStringAsync(geoUrl);
        var geoData = JsonSerializer.Deserialize<JsonElement[]>(geoResponse);

        if (geoData == null || geoData.Length == 0)
            return null;

        var lat = geoData[0].GetProperty("lat").GetDouble();
        var lon = geoData[0].GetProperty("lon").GetDouble();

        var nearestMainCity = await FindNearestMainCityAsync(lat, lon);

        var city = new City
        {
            CityName = cityName,
            Latitude = lat,
            Longitude = lon,
            IsMainCity = false,
            NearestMainCityId = nearestMainCity?.Id
        };

        city.Id = await _cityRepository.AddCityAsync(city);
        return city;
    }

    private async Task<City?> FindNearestMainCityAsync(double latitude, double longitude)
    {
        var mainCities = await _cityRepository.GetMainCitiesAsync(0, 1000);

        var nearest = mainCities
            .Select(c => new { City = c, Distance = CalculateDistance(latitude, longitude, c.Latitude, c.Longitude) })
            .OrderBy(x => x.Distance)
            .FirstOrDefault();

        if (nearest != null && nearest.Distance <= _radiusKm)
            return nearest.City;

        return null;
    }

    private static double CalculateDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double R = 6371;
        var dLat = ToRad(lat2 - lat1);
        var dLon = ToRad(lon2 - lon1);
        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(ToRad(lat1)) * Math.Cos(ToRad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return R * c;
    }

    private static double ToRad(double deg) => deg * Math.PI / 180;
    
    public async Task<IEnumerable<string>> SearchCitiesAsync(string query)
    {
        return await _cityRepository.SearchCitiesAsync(query);
    }
}