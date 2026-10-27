
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models;

public class CrearProductoRequest
{
    [NombreValido(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres")]
    public string Nombre { get; set; } = "";

    [StringLength(500, ErrorMessage = "La descripción no puede superar los 500 caracteres")]
    public string Descripcion { get; set; } = "";

    [PrecioValido(ErrorMessage = "El precio debe ser mayor que cero")]
    public decimal Precio { get; set; }

    [Range(0, 1000000,
        ErrorMessage = "El stock debe estar entre 0 y 1000000")]
    public int Stock { get; set; }

    public bool Activo { get; set; } = true;
    public int? CategoriaId { get; set; }
    public bool TieneNombreValido =>
        !string.IsNullOrWhiteSpace(Nombre);
}

public class PrecioValidoAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is not decimal precio)
            return false;

        return precio > 0 && precio <= 999999999;
    }
}
