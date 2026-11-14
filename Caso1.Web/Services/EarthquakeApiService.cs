using Caso1.Models.DTOs.Responses;
using System.Net;
using System.Net.Http.Json;

namespace Caso1.Web.Services;

public class EarthquakeApiService : IEarthquakeApiService
{
    private readonly HttpClient _httpClient;

    public EarthquakeApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<EarthquakeSearchResponseDto?> SearchAsync(
        string country,
        CancellationToken cancellationToken = default)
    {
        string url = "api/earthquakes/search?country="
            + Uri.EscapeDataString(country);

        HttpResponseMessage response = await _httpClient
            .GetAsync(url, cancellationToken);

        if (response.StatusCode == HttpStatusCode.NotFound)
        {
            return null;
        }

        response.EnsureSuccessStatusCode();

        return await response.Content
            .ReadFromJsonAsync<EarthquakeSearchResponseDto>(cancellationToken);
    }
}
