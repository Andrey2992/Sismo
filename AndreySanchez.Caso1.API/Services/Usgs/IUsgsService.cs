using Caso1.Models.DTOs.Usgs;

namespace Caso1.API.Services.Usgs;

public interface IUsgsService
{
    Task<UsgsResponseDto> GetEarthquakesAsync(
        DateTime startDate,
        DateTime endDate,
        CancellationToken cancellationToken = default);
}
