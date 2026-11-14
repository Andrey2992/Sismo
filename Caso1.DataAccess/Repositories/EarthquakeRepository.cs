using Caso1.DataAccess.Context;
using Caso1.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;

namespace Caso1.DataAccess.Repositories;

public class EarthquakeRepository : IEarthquakeRepository
{
    private readonly EarthquakeDbContext _context;

    public EarthquakeRepository(EarthquakeDbContext context)
    {
        _context = context;
    }

    public async Task<DateTime?> GetLatestEventTimeAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Earthquakes
            .AsNoTracking()
            .OrderByDescending(earthquake => earthquake.EventTimeUtc)
            .Select(earthquake => (DateTime?)earthquake.EventTimeUtc)
            .FirstOrDefaultAsync(cancellationToken);
    }   

    public async Task<HashSet<string>> GetExistingExternalIdsAsync(
        IEnumerable<string> ids,
        CancellationToken cancellationToken = default)
    {
        List<string> idsToSearch = ids.Distinct().ToList();

        List<string> existingIds = await _context.Earthquakes
            .Where(earthquake => idsToSearch.Contains(earthquake.ExternalId))
            .Select(earthquake => earthquake.ExternalId)
            .ToListAsync(cancellationToken);

        return existingIds.ToHashSet();
    }

    public async Task AddRangeAsync(
        IEnumerable<Earthquake> earthquakes,
        CancellationToken cancellationToken = default)
    {
        await _context.Earthquakes.AddRangeAsync(earthquakes, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Earthquake>> SearchByCountryAsync(
        string country,
        CancellationToken cancellationToken = default)
    {
        return await _context.Earthquakes
            .AsNoTracking()
            .Where(earthquake => earthquake.Place.Contains(country))
            .OrderByDescending(earthquake => earthquake.EventTimeUtc)
            .Take(500)
            .ToListAsync(cancellationToken);
    }
}
