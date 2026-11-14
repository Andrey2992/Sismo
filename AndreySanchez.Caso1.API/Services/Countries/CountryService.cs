using Caso1.Models.DTOs.Countries;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace Caso1.API.Services.Countries;

public class CountryService : ICountryService
{
    private readonly HttpClient _httpClient;
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;

    public CountryService(
        HttpClient httpClient,
        IMemoryCache cache,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _cache = cache;
        _configuration = configuration;
    }

    public async Task<CountryDto?> GetByNameAsync(
        string country,
        CancellationToken cancellationToken = default)
    {
        string cacheKey = "country-" + country.ToLower();

        if (_cache.TryGetValue(cacheKey, out CountryDto? cachedCountry)
            && cachedCountry != null)
        {
            return cachedCountry;
        }

        string baseUrl = _configuration["RestCountries:BaseUrl"] ?? string.Empty;
        string apiKey = _configuration["RestCountries:ApiKey"] ?? string.Empty;
        string url = baseUrl + Uri.EscapeDataString(country);

        using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);

        if (!string.IsNullOrWhiteSpace(apiKey)
            && apiKey != "PEGA_AQUI_TU_API_KEY")
        {
            request.Headers.Authorization =
                new AuthenticationHeaderValue("Bearer", apiKey);
        }

        using HttpResponseMessage response =
            await _httpClient.SendAsync(request, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            return null;
        }

        RestCountriesResponseDto? apiResponse = await response.Content
            .ReadFromJsonAsync<RestCountriesResponseDto>(cancellationToken);

        RestCountryDto? countryData = apiResponse?.Data.Objects.FirstOrDefault();

        if (countryData == null)
        {
            return null;
        }

        CountryDto result = ConvertToCountryDto(countryData);
        _cache.Set(cacheKey, result, TimeSpan.FromHours(12));

        return result;
    }

    private CountryDto ConvertToCountryDto(RestCountryDto countryData)
    {
        CountryDto country = new CountryDto();

        country.Name = countryData.Names.Common;
        country.OfficialName = countryData.Names.Official;
        country.Region = countryData.Region;
        country.Subregion = countryData.Subregion;
        country.Population = countryData.Population;

        CountryCapitalDto? capital = countryData.Capitals.FirstOrDefault();
        country.Capital = capital == null
            ? "Sin capital registrada"
            : capital.Name;

        country.FlagUrl = countryData.Flag.UrlSvg;
        country.Languages = countryData.Languages
            .Select(language => language.Name)
            .ToList();

        return country;
    }
}
