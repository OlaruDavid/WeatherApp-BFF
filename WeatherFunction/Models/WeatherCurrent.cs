namespace WeatherFunction.Models;

public class WeatherCurrent
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string WeatherData { get; set; } = string.Empty;
    public DateTime UpdatedAt { get; set; }
    public City City { get; set; } = null!;
}