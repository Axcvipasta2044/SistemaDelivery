
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models;

public class NombreValidoAttribute : ValidationAttribute
{
    public NombreValidoAttribute()
    {
        ErrorMessage = "El nombre no puede estar vacío ni contener solo espacios";
    }

    public override bool IsValid(object? value)
    {
        if (value is not string nombre)
            return false;

        return !string.IsNullOrWhiteSpace(nombre);
    }
}
