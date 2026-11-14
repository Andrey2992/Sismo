using Caso1.API.Services.Usgs;
using Caso1.DataAccess.Entities;
using Caso1.DataAccess.Repositories;
using Caso1.Models.DTOs.Usgs;

namespace Caso1.API.Services.Import;

public class EarthquakeImportService : IEarthquakeImportService
{
    private readonly IUsgsService _usgsService;
    private readonly IEarthquakeRepository _repository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<EarthquakeImportService> _logger;

    public EarthquakeImportService(
        IUsgsService usgsService,
        IEarthquakeRepository repository,
        IConfiguration configuration,
        ILogger<EarthquakeImportService> logger)
    {
        _usgsService = usgsService;
        _repository = repository;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<int> ImportNewEarthquakesAsync(
        CancellationToken cancellationToken = default)
    {
        DateTime firstDate = _configuration
            .GetValue<DateTime>("Usgs:StartDate");

        DateTime lastDate = _configuration
            .GetValue<DateTime>("Usgs:EndDate");

        DateTime? latestSavedDate = await _repository
            .GetLatestEventTimeAsync(cancellationToken);

        DateTime currentDate = latestSavedDate == null
            ? firstDate
            : latestSavedDate.Value.Date.AddDays(-1);

        int savedCount = 0;

        while (currentDate < lastDate)
        {
            DateTime monthEnd = currentDate.AddMonths(1);

            if (monthEnd > lastDate)
            {
                monthEnd = lastDate;
            }

            UsgsResponseDto response = await _usgsService
                .GetEarthquakesAsync(currentDate, monthEnd, cancellationToken);

            List<string> receivedIds = response.Features
                .Select(feature => feature.Id)
                .ToList();

            HashSet<string> existingIds = await _repository
                .GetExistingExternalIdsAsync(receivedIds, cancellationToken);

            List<Earthquake> newEarthquakes = new List<Earthquake>();

            foreach (UsgsFeatureDto feature in response.Features)
            {
                if (!existingIds.Contains(feature.Id))
                {
                    Earthquake earthquake = ConvertToEntity(feature);
                    newEarthquakes.Add(earthquake);
                }
            }

            if (newEarthquakes.Count > 0)
            {
                await _repository.AddRangeAsync(newEarthquakes, cancellationToken);
            }

            savedCount += newEarthquakes.Count;

            _logger.LogInformation(
                "Se guardaron {Count} sismos nuevos del periodo {Start} - {End}",
                newEarthquakes.Count,
                currentDate,
                monthEnd);

            currentDate = monthEnd;
        }

        return savedCount;
    }

    private Earthquake ConvertToEntity(UsgsFeatureDto feature)
    {
        Earthquake earthquake = new Earthquake();

        earthquake.ExternalId = feature.Id;
        earthquake.Magnitude = feature.Properties.Magnitude;
        earthquake.Place = feature.Properties.Place;
        earthquake.EventTimeUtc = DateTimeOffset
            .FromUnixTimeMilliseconds(feature.Properties.EventTime)
            .UtcDateTime;
        earthquake.UpdatedTimeUtc = DateTimeOffset
            .FromUnixTimeMilliseconds(feature.Properties.UpdatedTime)
            .UtcDateTime;
        earthquake.Longitude = feature.Geometry.Coordinates.ElementAtOrDefault(0);
        earthquake.Latitude = feature.Geometry.Coordinates.ElementAtOrDefault(1);
        earthquake.DepthKm = feature.Geometry.Coordinates.ElementAtOrDefault(2);
        earthquake.DetailUrl = feature.Properties.DetailUrl;
        earthquake.Status = feature.Properties.Status;
        earthquake.Tsunami = feature.Properties.Tsunami == 1;

        return earthquake;
    }
}
