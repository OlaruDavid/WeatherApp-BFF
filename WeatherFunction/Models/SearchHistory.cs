namespace WeatherFunction.Models;

public class SearchHistory
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string CityName { get; set; } = string.Empty;
    public DateTime SearchedAt { get; set; }
    public City City { get; set; } = null!;
}