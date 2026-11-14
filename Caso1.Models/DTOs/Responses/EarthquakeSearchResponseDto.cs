using Caso1.Models.DTOs.Countries;

namespace Caso1.Models.DTOs.Responses;

public class EarthquakeSearchResponseDto
{
    public string SearchedCountry { get; set; } = string.Empty;
    public CountryDto? Country { get; set; }
    public List<EarthquakeDto> Earthquakes { get; set; } = [];
    public int Total => Earthquakes.Count;
}
