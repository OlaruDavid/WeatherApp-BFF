namespace WeatherFunction.Models;

public class City
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsMainCity { get; set; }
    public int? NearestMainCityId { get; set; }
    public City? NearestMainCity { get; set; }
    public ICollection<WeatherCurrent> WeatherCurrents { get; set; } = new List<WeatherCurrent>();
    public ICollection<WeatherHistory> WeatherHistories { get; set; } = new List<WeatherHistory>();
    public ICollection<SearchHistory> SearchHistories { get; set; } = new List<SearchHistory>();
}