namespace Caso1.Models.DTOs.Responses;

public class EarthquakeDto
{
    public string ExternalId { get; set; } = string.Empty;
    public decimal? Magnitude { get; set; }
    public string Place { get; set; } = string.Empty;
    public DateTime EventTimeUtc { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double DepthKm { get; set; }
    public string DetailUrl { get; set; } = string.Empty;
    public bool Tsunami { get; set; }
}
