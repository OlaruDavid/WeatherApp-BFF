namespace WeatherApi.DTOs;

public class CityDto
{
    public int Id { get; set; }
    public string CityName { get; set; } = string.Empty;
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Temperature { get; set; }
    public double Humidity { get; set; }
}