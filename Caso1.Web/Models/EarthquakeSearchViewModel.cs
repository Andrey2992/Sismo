using Caso1.Models.DTOs.Responses;
using System.ComponentModel.DataAnnotations;
namespace Caso1.Web.Models;
public class EarthquakeSearchViewModel
{
    [Required(ErrorMessage = "Escribe el nombre de un país.")]
    [StringLength(100, ErrorMessage = "Máximo 100 caracteres.")]
    [RegularExpression(@"^[A-Za-zÁÉÍÓÚáéíóúÑñÜü\s-]+$", ErrorMessage = "Solo letras, espacios y guiones.")]
    public string Country { get; set; } = string.Empty;
    public EarthquakeSearchResponseDto? Result { get; set; }
    public string? AlertMessage { get; set; }
    public string AlertType { get; set; } = "error";
}
