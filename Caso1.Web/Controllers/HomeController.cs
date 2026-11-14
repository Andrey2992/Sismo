using Caso1.Web.Models;
using Caso1.Web.Services;
using Microsoft.AspNetCore.Mvc;

namespace Caso1.Web.Controllers;

public class HomeController : Controller
{
    private readonly IEarthquakeApiService _apiService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        IEarthquakeApiService apiService,
        ILogger<HomeController> logger)
    {
        _apiService = apiService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Index()
    {
        EarthquakeSearchViewModel model = new EarthquakeSearchViewModel();
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Index(
        EarthquakeSearchViewModel model,
        CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            model.AlertMessage = "Revisa el país escrito";
            return View(model);
        }

        try
        {
            model.Result = await _apiService
                .SearchAsync(model.Country.Trim(), cancellationToken);

            if (model.Result == null)
            {
                model.AlertMessage = "No encontramos ese país";
            }
            else if (model.Result.Earthquakes.Count == 0)
            {
                model.AlertType = "info";
                model.AlertMessage =
                    "El país existe, pero no hay sismos para mostrar";
            }
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error al consultar la API");
            model.AlertMessage =
                "No pudimos conectar con la API, verifica que esté ejecutándose";
        }

        return View(model);
    }
}
