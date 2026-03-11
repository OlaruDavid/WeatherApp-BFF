using Microsoft.AspNetCore.Mvc;
using WeatherApi.Interfaces;

namespace WeatherApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherController : ControllerBase
{
    private readonly ICityService _cityService;
    private readonly IWeatherService _weatherService;
    private readonly ILogger<WeatherController> _logger;

    public WeatherController(
        ICityService cityService,
        IWeatherService weatherService,
        ILogger<WeatherController> logger)
    {
        _cityService = cityService;
        _weatherService = weatherService;
        _logger = logger;
    }

    [HttpGet("cities")]
    public async Task<IActionResult> GetCities([FromQuery] int skip = 0, [FromQuery] int take = 5)
    {
        try
        {
            var cities = await _cityService.GetMainCitiesAsync(skip, take);
            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Eroare la obtinerea oraselor");
            return StatusCode(500, "A aparut o eroare!");
        }
    }

    [HttpGet("{cityName}")]
    public async Task<IActionResult> GetWeather(string cityName)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return BadRequest("Numele orasului nu poate fi gol!");

        if (cityName.Length > 100)
            return BadRequest("Numele orasului este prea lung!");

        if (!System.Text.RegularExpressions.Regex.IsMatch(cityName, @"^[a-zA-ZăâîșțĂÂÎȘȚ\s\-]+$"))
            return BadRequest("Numele orasului contine caractere invalide!");

        try
        {
            var weather = await _cityService.GetCityWeatherAsync(cityName);
            if (weather == null)
                return NotFound($"Orasul {cityName} nu a fost gasit!");

            return Ok(weather);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Eroare la obtinerea vremii pentru {CityName}", cityName);
            return StatusCode(500, "A aparut o eroare!");
        }
    }

    [HttpGet("{cityName}/history")]
    public async Task<IActionResult> GetWeatherHistory(string cityName, [FromQuery] int days = 1)
    {
        if (string.IsNullOrWhiteSpace(cityName))
            return BadRequest("Numele orasului nu poate fi gol!");

        try
        {
            var history = await _weatherService.GetWeatherHistoryAsync(cityName, days);
            return Ok(history);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Eroare la obtinerea istoricului pentru {CityName}", cityName);
            return StatusCode(500, "A aparut o eroare!");
        }
    }
    
    [HttpGet("search")]
    public async Task<IActionResult> SearchCities([FromQuery] string query)
    {
        if (string.IsNullOrWhiteSpace(query) || query.Length < 2)
            return BadRequest("Minim 2 caractere!");

        try
        {
            var cities = await _cityService.SearchCitiesAsync(query);
            return Ok(cities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Eroare la cautarea oraselor");
            return StatusCode(500, "A aparut o eroare!");
        }
    }
}