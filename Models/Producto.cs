
using System.ComponentModel.DataAnnotations;

namespace TiendaAPI.Models;

public class Producto
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = "";

    [StringLength(500)]
    public string Descripcion { get; set; } = "";

    [Range(typeof(decimal), "0.01", "999999999")]
    public decimal Precio { get; set; }

    [Range(0, 1000000)]
    public int Stock { get; set; }

    public string? Imagen { get; set; }

    public bool Activo { get; set; } = true;

    // Clave foránea de la categoría
    public int? CategoriaId { get; set; }

    // Navegación a la categoría
    public Categoria? Categoria { get; set; }
}
