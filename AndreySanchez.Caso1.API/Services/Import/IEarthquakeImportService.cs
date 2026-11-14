namespace Caso1.API.Services.Import;

public interface IEarthquakeImportService
{
    Task<int> ImportNewEarthquakesAsync(
        CancellationToken cancellationToken = default);
}
