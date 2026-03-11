using Dapper;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Persistence;

namespace WeatherApi.Repositories;

public class CityRepository : ICityRepository
{
    private readonly DapperContext _context;

    public CityRepository(DapperContext context)
    {
        _context = context;
    }
    
    public async Task<IEnumerable<City>> GetMainCitiesAsync(int skip, int take)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<City>(
            @"SELECT ""Id"", ""CityName"", ""Latitude"", ""Longitude"", ""IsMainCity"", ""NearestMainCityId""
              FROM ""Cities""
              WHERE ""IsMainCity"" = true
              ORDER BY ""CityName""
              OFFSET @Skip LIMIT @Take",
            new { Skip = skip, Take = take });
    }

    public async Task<City?> FindByNameAsync(string cityName)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryFirstOrDefaultAsync<City>(
            @"SELECT ""Id"", ""CityName"", ""Latitude"", ""Longitude"", ""IsMainCity"", ""NearestMainCityId""
              FROM ""Cities""
              WHERE LOWER(""CityName"") = LOWER(@CityName)",
            new { CityName = cityName });
    }

    public async Task<int> AddCityAsync(City city)
    {
        using var conn = _context.CreateConnection();
        return await conn.ExecuteScalarAsync<int>(
            @"INSERT INTO ""Cities"" (""CityName"", ""Latitude"", ""Longitude"", ""IsMainCity"", ""NearestMainCityId"")
              VALUES (@CityName, @Latitude, @Longitude, @IsMainCity, @NearestMainCityId)
              RETURNING ""Id""",
            city);
    }
    public async Task<IEnumerable<string>> SearchCitiesAsync(string query)
    {
        using var conn = _context.CreateConnection();
        return await conn.QueryAsync<string>(
            @"SELECT ""CityName"" FROM ""Cities""
          WHERE LOWER(""CityName"") LIKE LOWER(@Query)
          ORDER BY ""CityName""
          LIMIT 5",
            new { Query = query + "%" });
    }
}