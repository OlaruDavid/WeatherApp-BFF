using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WeatherFunction.Data;
using WeatherFunction.Interfaces;
using WeatherFunction.Models;

namespace WeatherFunction.Services;

public class WeatherService : IWeatherService
{
    private readonly AppDbContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public WeatherService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _httpClient = new HttpClient();
        _apiKey = configuration["OpenWeatherMapApiKey"] ?? throw new Exception("OpenWeatherMapApiKey lipseste!");
    }

    public async Task<string> GetCurrentWeatherAsync(string cityName)
    {
        var city = await _context.Cities
            .Include(c => c.NearestMainCity)
            .FirstOrDefaultAsync(c => c.CityName.ToLower() == cityName.ToLower());

        if (city == null)
            throw new Exception($"Orasul {cityName} nu a fost gasit!");

        if (!city.IsMainCity && city.NearestMainCityId != null)
        {
            var mainCityWeather = await _context.WeatherCurrents
                .FirstOrDefaultAsync(w => w.CityId == city.NearestMainCityId);

            if (mainCityWeather != null)
                return mainCityWeather.WeatherData;

            throw new Exception($"Datele meteo pentru zona {cityName} nu sunt inca disponibile. Incearca din nou in cateva minute!");
        }

        if (city.IsMainCity)
        {
            var current = await _context.WeatherCurrents
                .FirstOrDefaultAsync(w => w.CityId == city.Id);

            if (current != null)
                return current.WeatherData;

            throw new Exception($"Datele meteo pentru {cityName} nu sunt inca disponibile. Incearca din nou in cateva minute!");
        }

        var existing = await _context.WeatherCurrents
            .FirstOrDefaultAsync(w => w.CityId == city.Id);

        if (existing != null)
            return existing.WeatherData;

        return await FetchAndSaveWeatherAsync(city);
    }

    public async Task<IEnumerable<string>> GetWeatherHistoryAsync(string cityName, int days)
    {
        var city = await _context.Cities
            .FirstOrDefaultAsync(c => c.CityName.ToLower() == cityName.ToLower());

        if (city == null)
            throw new Exception($"Orasul {cityName} nu a fost gasit!");

        var targetCityId = (!city.IsMainCity && city.NearestMainCityId != null)
            ? city.NearestMainCityId.Value
            : city.Id;

        var from = DateTime.UtcNow.AddDays(-days);

        return await _context.WeatherHistories
            .Where(w => w.CityId == targetCityId && w.RecordedAt >= from)
            .OrderByDescending(w => w.RecordedAt)
            .Select(w => w.WeatherData)
            .ToListAsync();
    }

    public async Task<string> FetchAndSaveWeatherAsync(City city)
    {
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={city.CityName},RO&appid={_apiKey}&units=metric&lang=ro";
        var response = await _httpClient.GetStringAsync(url);

        var existing = await _context.WeatherCurrents
            .FirstOrDefaultAsync(w => w.CityId == city.Id);

        if (existing != null)
        {
            _context.WeatherHistories.Add(new WeatherHistory
            {
                CityId = city.Id,
                WeatherData = existing.WeatherData,
                RecordedAt = existing.UpdatedAt
            });
            _context.WeatherCurrents.Remove(existing);
        }

        _context.WeatherCurrents.Add(new WeatherCurrent
        {
            CityId = city.Id,
            WeatherData = response,
            UpdatedAt = DateTime.UtcNow
        });

        await _context.SaveChangesAsync();
        return response;
    }
}