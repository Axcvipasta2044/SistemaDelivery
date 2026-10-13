namespace TiendaAPI.Models;

public class Producto
{
    public string Id { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Descripcion { get; set; } = "";
    public decimal Precio { get; set; }
    public int Stock { get; set; }
    public string Imagen { get; set; } = "";
    public bool Activo { get; set; }
}
