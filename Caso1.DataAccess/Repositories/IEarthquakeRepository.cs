using Caso1.DataAccess.Entities;

namespace Caso1.DataAccess.Repositories;

public interface IEarthquakeRepository
{
    Task<DateTime?> GetLatestEventTimeAsync(CancellationToken cancellationToken = default);
    Task<HashSet<string>> GetExistingExternalIdsAsync(IEnumerable<string> ids, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<Earthquake> earthquakes, CancellationToken cancellationToken = default);
    Task<List<Earthquake>> SearchByCountryAsync(string country, CancellationToken cancellationToken = default);
}
