
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly TiendaDbContext _context;

    public ProductosController(TiendaDbContext context)
    {
        _context = context;
    }

    // GET: api/productos

[HttpGet]
public async Task<IActionResult> ObtenerProductos()
{
    var productos = await _context.Productos
        .AsNoTracking()
        .Select(p => new
        {
            p.Id,
            p.Nombre,
            p.Descripcion,
            p.Precio,
            p.Stock,
            p.Imagen,
            p.Activo,
            p.CategoriaId,
            Categoria = p.Categoria == null
                ? null
                : new
                {
                    p.Categoria.Id,
                    p.Categoria.Nombre,
                    p.Categoria.Descripcion,
                    p.Categoria.Activo
                }
        })
        .ToListAsync();

    return Ok(productos);
}

    // GET: api/productos/1
[HttpGet("{id:int}")]
public async Task<IActionResult> ObtenerProducto(int id)
{
    var producto = await _context.Productos
        .Include(p => p.Categoria)
        .AsNoTracking()
        .FirstOrDefaultAsync(p => p.Id == id);

    if (producto == null)
    {
        return NotFound(new
        {
            mensaje = "Producto no encontrado"
        });
    }

    return Ok(producto);
}

    // POST: api/productos
    [HttpPost]
    public async Task<IActionResult> CrearProducto(
        [FromBody] CrearProductoRequest solicitud)
    {
        if (solicitud.CategoriaId.HasValue)
{
    var categoriaExiste = await _context.Categorias
        .AnyAsync(c => c.Id == solicitud.CategoriaId.Value);

    if (!categoriaExiste)
    {
        return BadRequest(new
        {
            mensaje = "La categoría indicada no existe"
        });
    }
}
        var producto = new Producto
        {
            Nombre = solicitud.Nombre,
            Descripcion = solicitud.Descripcion,
            Precio = solicitud.Precio,
            Stock = solicitud.Stock,
            Activo = solicitud.Activo,
            CategoriaId = solicitud.CategoriaId
        };

        _context.Productos.Add(producto);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerProducto),
            new { id = producto.Id },
            producto
        );
    }

    // PUT: api/productos/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarProducto(
        int id,
        [FromBody] CrearProductoRequest solicitud)
    {
        if (solicitud.CategoriaId.HasValue)
{
    var categoriaExiste = await _context.Categorias
        .AnyAsync(c => c.Id == solicitud.CategoriaId.Value);

    if (!categoriaExiste)
    {
        return BadRequest(new
        {
            mensaje = "La categoría indicada no existe"
        });
    }
}
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

        producto.Nombre = solicitud.Nombre;
        producto.Descripcion = solicitud.Descripcion;
        producto.Precio = solicitud.Precio;
        producto.Stock = solicitud.Stock;
        producto.Activo = solicitud.Activo;

        await _context.SaveChangesAsync();

        return Ok(producto);
        
    }

    // DELETE: api/productos/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarProducto(int id)
    {
        var producto = await _context.Productos.FindAsync(id);

        if (producto == null)
        {
            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

        _context.Productos.Remove(producto);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensaje = "Producto eliminado correctamente",
            id
        });
    }
}
