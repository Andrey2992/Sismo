using System.Text.Json.Serialization;
namespace Caso1.Models.DTOs.Countries;

public class RestCountriesResponseDto
{
    [JsonPropertyName("data")]
    public RestCountriesDataDto Data { get; set; } = new();
}
public class RestCountriesDataDto
{
    [JsonPropertyName("objects")]
    public List<RestCountryDto> Objects { get; set; } = [];
}
public class RestCountryDto
{
    [JsonPropertyName("names")]
    public CountryNameDto Names { get; set; } = new();
    [JsonPropertyName("capitals")]
    public List<CountryCapitalDto> Capitals { get; set; } = [];
    [JsonPropertyName("region")]
    public string Region { get; set; } = string.Empty;
    [JsonPropertyName("subregion")]
    public string Subregion { get; set; } = string.Empty;
    [JsonPropertyName("population")]
    public long Population { get; set; }
    [JsonPropertyName("flag")]
    public CountryFlagDto Flag { get; set; } = new();
    [JsonPropertyName("languages")]
    public List<CountryLanguageDto> Languages { get; set; } = [];
}
public class CountryNameDto
{
    [JsonPropertyName("common")]
    public string Common { get; set; } = string.Empty;
    [JsonPropertyName("official")]
    public string Official { get; set; } = string.Empty;
}
public class CountryCapitalDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
public class CountryFlagDto
{
    [JsonPropertyName("url_png")]
    public string UrlPng { get; set; } = string.Empty;
    [JsonPropertyName("url_svg")]
    public string UrlSvg { get; set; } = string.Empty;
}
public class CountryLanguageDto
{
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
}
