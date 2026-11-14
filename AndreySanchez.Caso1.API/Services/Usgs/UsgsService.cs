using Caso1.Models.DTOs.Usgs;
using Microsoft.Extensions.Caching.Memory;
using System.Globalization;
using System.Net.Http.Json;

namespace Caso1.API.Services.Usgs;

public class UsgsService : IUsgsService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    public UsgsService(
        HttpClient httpClient,
        IMemoryCache cache,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<UsgsResponseDto> GetEarthquakesAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = $"usgs-{startDate:yyyy-MM-dd}-{endDate:yyyy-MM-dd}";

        if (_cache.TryGetValue(cacheKey, out UsgsResponseDto? cachedResponse)
            && cachedResponse != null)
        {
            return cachedResponse;
        }

        string baseUrl = _configuration["Usgs:BaseUrl"] ?? string.Empty;
        decimal minimumMagnitude = _configuration.GetValue<decimal>("Usgs:MinimumMagnitude");
        string magnitudeText = minimumMagnitude.ToString(CultureInfo.InvariantCulture);

        string url = baseUrl
            + "?format=geojson"
            + $"&starttime={startDate:yyyy-MM-dd}"
            + $"&endtime={endDate:yyyy-MM-dd}"
            + $"&minmagnitude={magnitudeText}"
            + "&orderby=time-asc&limit=20000";

        UsgsResponseDto? response = await _httpClient
            .GetFromJsonAsync<UsgsResponseDto>(url, cancellationToken);

        if (response == null)
        {
            response = new UsgsResponseDto();
        }

        _cache.Set(cacheKey, response, TimeSpan.FromMinutes(30));
        return response;
    }
}
