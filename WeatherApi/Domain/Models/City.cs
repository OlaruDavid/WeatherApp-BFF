namespace WeatherApi.Models;

public class City
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool IsMainCity { get; set; }
    public int? NearestMainCityId { get; set; }
}