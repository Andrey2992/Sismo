using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Caso1.Models.DTOs.Usgs;

public class UsgsFeatureDto
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("type")]
    public string Type { get; set; } = string.Empty;

    [JsonPropertyName("properties")]
    public UsgsPropertiesDto Properties { get; set; } = new();

    [JsonPropertyName("geometry")]
    public UsgsGeometryDto Geometry { get; set; } = new();
}