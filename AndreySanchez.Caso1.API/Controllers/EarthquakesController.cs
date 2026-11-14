using Caso1.API.Services.Countries;
using Caso1.DataAccess.Entities;
using Caso1.DataAccess.Repositories;
using Caso1.Models.DTOs.Countries;
using Caso1.Models.DTOs.Responses;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;

namespace Caso1.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EarthquakesController : ControllerBase
{
    private readonly IEarthquakeRepository _repository;
    private readonly ICountryService _countryService;

    public EarthquakesController(
        IEarthquakeRepository repository,
        ICountryService countryService)
    {
        _repository = repository;
        _countryService = countryService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<EarthquakeSearchResponseDto>> Search(
        [FromQuery] string country,
        CancellationToken cancellationToken)
    {
        if (!IsValidCountryName(country))
        {
            return BadRequest(new
            {
                message = "Escriba un nombre de país válido"
            });
        }

        string countryToSearch = country.Trim();

        CountryDto? countryInfo = await _countryService
            .GetByNameAsync(countryToSearch, cancellationToken);

        if (countryInfo == null)
        {
            return NotFound(new
            {
                message = "No se encontró ese país"
            });
        }

        List<Earthquake> earthquakes = await _repository
            .SearchByCountryAsync(countryInfo.Name, cancellationToken);

        List<EarthquakeDto> earthquakeDtos = new List<EarthquakeDto>();

        foreach (Earthquake earthquake in earthquakes)
        {
            earthquakeDtos.Add(ConvertToDto(earthquake));
        }

        EarthquakeSearchResponseDto result = new EarthquakeSearchResponseDto();
        result.SearchedCountry = countryToSearch;
        result.Country = countryInfo;
        result.Earthquakes = earthquakeDtos;

        return Ok(result);
    }

    private bool IsValidCountryName(string country)
    {
        if (string.IsNullOrWhiteSpace(country))
        {
            return false;
        }

        if (country.Length > 100)
        {
            return false;
        }

        return Regex.IsMatch(
            country,
            @"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s-]+$");
    }

    private EarthquakeDto ConvertToDto(Earthquake earthquake)
    {
        EarthquakeDto dto = new EarthquakeDto();

        dto.ExternalId = earthquake.ExternalId;
        dto.Magnitude = earthquake.Magnitude;
        dto.Place = earthquake.Place;
        dto.EventTimeUtc = earthquake.EventTimeUtc;
        dto.Longitude = earthquake.Longitude;
        dto.Latitude = earthquake.Latitude;
        dto.DepthKm = earthquake.DepthKm;
        dto.DetailUrl = earthquake.DetailUrl;
        dto.Tsunami = earthquake.Tsunami;

        return dto;
    }
}
