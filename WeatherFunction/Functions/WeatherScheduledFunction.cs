using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using WeatherFunction.Data;
using Microsoft.EntityFrameworkCore;
using WeatherFunction.Interfaces;

namespace WeatherFunction.Functions;

public class WeatherScheduledFunction
{
    private readonly AppDbContext _context;
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherScheduledFunction> _logger;

    public WeatherScheduledFunction(
        AppDbContext context,
        IWeatherService weatherService,
        ILogger<WeatherScheduledFunction> logger)
    {
        _context = context;
        _weatherService = weatherService;
        _logger = logger;
    }

    [Function("UpdateWeather")]
    public async Task Run([TimerTrigger("0 0 * * * *")] TimerInfo timerInfo)
    {
        _logger.LogInformation("Actualizare vreme pornita la: {Time}", DateTime.UtcNow);

        var mainCities = await _context.Cities
            .Where(c => c.IsMainCity)
            .ToListAsync();

        var success = 0;
        var failed = 0;

        foreach (var city in mainCities)
        {
            try
            {
                await _weatherService.FetchAndSaveWeatherAsync(city);
                _logger.LogInformation("✅ Actualizat: {CityName}", city.CityName);
                success++;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Eroare la actualizarea pentru {CityName}", city.CityName);
                failed++;
            }
        }

        _logger.LogInformation(
            "Actualizare finalizata. ✅ {Success} orase actualizate, ❌ {Failed} erori.",
            success, failed);
    }
}