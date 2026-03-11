namespace WeatherApi.Interfaces;

public interface ICityRepository
{
    Task<IEnumerable<Models.City>> GetMainCitiesAsync(int skip, int take);
    Task<Models.City?> FindByNameAsync(string cityName);
    Task<int> AddCityAsync(Models.City city);
    Task<IEnumerable<string>> SearchCitiesAsync(string query);

}