using Dapper;
using WeatherApi.Interfaces;
using WeatherApi.Models;
using WeatherApi.Persistence;

namespace WeatherApi.Repositories;

public class SearchHistoryRepository : ISearchHistoryRepository
{
    private readonly DapperContext _context;

    public SearchHistoryRepository(DapperContext context)
    {
        _context = context;
    }

    public async Task AddSearchAsync(SearchHistory search)
    {
        using var conn = _context.CreateConnection();
        await conn.ExecuteAsync(
            @"INSERT INTO ""SearchHistories"" (""CityId"", ""CityName"", ""SearchedAt"")
              VALUES (@CityId, @CityName, @SearchedAt)",
            search);
    }
}