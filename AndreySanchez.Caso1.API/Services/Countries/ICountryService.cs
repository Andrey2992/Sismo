using Caso1.Models.DTOs.Countries;

namespace Caso1.API.Services.Countries;

public interface ICountryService
{
    Task<CountryDto?> GetByNameAsync(
        string country,
        CancellationToken cancellationToken = default);
}
