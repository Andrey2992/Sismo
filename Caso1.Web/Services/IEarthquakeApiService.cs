using Caso1.Models.DTOs.Responses;

namespace Caso1.Web.Services;

public interface IEarthquakeApiService
{
    Task<EarthquakeSearchResponseDto?> SearchAsync(
        string country,
        CancellationToken cancellationToken = default);
}
