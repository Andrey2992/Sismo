using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace Caso1.Models.DTOs.Usgs
{
    public class UsgsResponseDto
    {
        [JsonPropertyName("type")]
        public string Type { get; set; } = string.Empty;

        [JsonPropertyName("features")]
        public List<UsgsFeatureDto> Features { get; set; } = [];
    }
}