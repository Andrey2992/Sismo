namespace Caso1.DataAccess.Entities;

public class Earthquake
{
    public int Id { get; set; }
    public string ExternalId { get; set; } = string.Empty;
    public decimal? Magnitude { get; set; }
    public string Place { get; set; } = string.Empty;
    public DateTime EventTimeUtc { get; set; }
    public DateTime UpdatedTimeUtc { get; set; }
    public double Longitude { get; set; }
    public double Latitude { get; set; }
    public double DepthKm { get; set; }
    public string DetailUrl { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool Tsunami { get; set; }
}
