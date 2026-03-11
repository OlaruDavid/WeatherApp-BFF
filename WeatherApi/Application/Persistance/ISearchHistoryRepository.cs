namespace WeatherApi.Interfaces;

public interface ISearchHistoryRepository
{
    Task AddSearchAsync(Models.SearchHistory search);
}