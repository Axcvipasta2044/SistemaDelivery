
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly TiendaDbContext _context;

    public CategoriasController(TiendaDbContext context)
    {
        _context = context;
    }

    // GET: api/categorias
    [HttpGet]
    public async Task<IActionResult> ObtenerCategorias()
    {
        var categorias = await _context.Categorias
            .AsNoTracking()
            .ToListAsync();

        return Ok(categorias);
    }

    // GET: api/categorias/1
    [HttpGet("{id:int}")]
    public async Task<IActionResult> ObtenerCategoria(int id)
    {
        var categoria = await _context.Categorias
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
        {
            return NotFound(new
            {
                mensaje = "Categoría no encontrada"
            });
        }

        return Ok(categoria);
    }

    // POST: api/categorias
    [HttpPost]
    public async Task<IActionResult> CrearCategoria(
        [FromBody] Categoria solicitud)
    {
        var categoria = new Categoria
        {
            Nombre = solicitud.Nombre,
            Descripcion = solicitud.Descripcion,
            Activo = solicitud.Activo
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(ObtenerCategoria),
            new { id = categoria.Id },
            categoria
        );
    }

    // PUT: api/categorias/1
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarCategoria(
        int id,
        [FromBody] Categoria solicitud)
    {
        var categoria = await _context.Categorias.FindAsync(id);

        if (categoria == null)
        {
            return NotFound(new
            {
                mensaje = "Categoría no encontrada"
            });
        }

        categoria.Nombre = solicitud.Nombre;
        categoria.Descripcion = solicitud.Descripcion;
        categoria.Activo = solicitud.Activo;

        await _context.SaveChangesAsync();

        return Ok(categoria);
    }

    // DELETE: api/categorias/1
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> EliminarCategoria(int id)
    {
        var categoria = await _context.Categorias
            .Include(c => c.Productos)
            .FirstOrDefaultAsync(c => c.Id == id);

        if (categoria == null)
        {
            return NotFound(new
            {
                mensaje = "Categoría no encontrada"
            });
        }

        if (categoria.Productos.Any())
        {
            return Conflict(new
            {
                mensaje = "No se puede eliminar una categoría que tiene productos."
            });
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return Ok(new
        {
            mensaje = "Categoría eliminada correctamente",
            id
        });
    }
}
