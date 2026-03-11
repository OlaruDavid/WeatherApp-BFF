namespace WeatherApi.Models;

public class WeatherHistory
{
    public int Id { get; set; }
    public int CityId { get; set; }
    public string WeatherData { get; set; } = string.Empty;
    public DateTime RecordedAt { get; set; }
}