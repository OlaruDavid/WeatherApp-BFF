using Dapper;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Persistence;

namespace WeatherApi.Repositories;

public class WeatherRepository : IWeatherRepository
{
    private readonly DapperContext _context;

    public WeatherRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task<WeatherCurrent?> GetCurrentWeatherAsync(int cityId)
    {
        using var conn = _context.CreateConnection();

        return await conn.QueryFirstOrDefaultAsync<WeatherCurrent>(
            @"SELECT ""Id"", ""CityId"", ""WeatherData"", ""UpdatedAt""
              FROM ""WeatherCurrents""
              WHERE ""CityId"" = @CityId",
            new { CityId = cityId });
    }

    public async Task<IEnumerable<WeatherHistory>> GetWeatherHistoryAsync(int cityId, int days)
    {
        using var conn = _context.CreateConnection();

        return await conn.QueryAsync<WeatherHistory>(
            @"SELECT ""Id"", ""CityId"", ""WeatherData"", ""RecordedAt""
              FROM ""WeatherHistories""
              WHERE ""CityId"" = @CityId
              AND ""RecordedAt"" >= @From
              ORDER BY ""RecordedAt"" DESC",
            new { CityId = cityId, From = DateTime.UtcNow.AddDays(-days) });
    }
    
}