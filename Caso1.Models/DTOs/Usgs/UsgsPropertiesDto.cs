using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Caso1.Models.DTOs.Usgs;

public class UsgsPropertiesDto
{
    [JsonPropertyName("mag")]
    public decimal? Magnitude { get; set; }

    [JsonPropertyName("place")]
    public string Place { get; set; } = string.Empty;

    [JsonPropertyName("time")]
    public long EventTime { get; set; }

    [JsonPropertyName("updated")]
    public long UpdatedTime { get; set; }

    [JsonPropertyName("url")]
    public string Url { get; set; } = string.Empty;

    [JsonPropertyName("detail")]
    public string DetailUrl { get; set; } = string.Empty;

    [JsonPropertyName("status")]
    public string Status { get; set; } = string.Empty;

    [JsonPropertyName("tsunami")]
    public int Tsunami { get; set; }

    [JsonPropertyName("sig")]
    public int Significance { get; set; }

    [JsonPropertyName("net")]
    public string Network { get; set; } = string.Empty;

    [JsonPropertyName("code")]
    public string Code { get; set; } = string.Empty;

    [JsonPropertyName("magType")]
    public string MagnitudeType { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string EventType { get; set; } = string.Empty;
}