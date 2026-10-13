
using Microsoft.AspNetCore.Mvc;
using TiendaAPI.Services;
using TiendaAPI.Models;

namespace TiendaAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly ProductoService _productoService;

    public ProductosController(ProductoService productoService)
    {
        _productoService = productoService;
    }

    // GET: api/productos
    [HttpGet]
    public async Task<IActionResult> ObtenerProductos()
    {
        var productos = await _productoService.ObtenerProductos();

        return Ok(productos);
    }

    // GET: api/productos/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> ObtenerProducto(string id)
    {
        var producto = await _productoService.ObtenerProducto(id);

        if (producto == null)
        {
            return NotFound(new
            {
                mensaje = "Producto no encontrado"
            });
        }

        return Ok(producto);
    }

    // GET: api/productos/probar-auth
    [HttpGet("probar-auth")]
    public async Task<IActionResult> ProbarAuth(
        [FromServices] PocketBaseAuthService authService)
    {
        try
        {
            await authService.ObtenerToken();

            return Ok(new
            {
                autenticado = true,
                mensaje = "La API se autenticó correctamente con PocketBase"
            });
        }
        catch
        {
            return StatusCode(500, new
            {
                autenticado = false,
                mensaje = "No se pudo autenticar con PocketBase"
            });
        }
    }

    // POST: api/productos
    [HttpPost]
    public async Task<IActionResult> CrearProducto(
        [FromBody] CrearProductoRequest producto,
        [FromServices] PocketBaseAuthService authService)
    {
        var token = await authService.ObtenerToken();

        var nuevoProducto =
            await _productoService.CrearProducto(producto, token);

        if (nuevoProducto == null)
        {
            return StatusCode(500, new
            {
                mensaje = "No se pudo crear el producto"
            });
        }

        return CreatedAtAction(
            nameof(ObtenerProducto),
            new { id = nuevoProducto.Id },
            nuevoProducto
        );
    }

    // PUT: api/productos/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> ActualizarProducto(
        string id,
        [FromBody] CrearProductoRequest producto,
        [FromServices] PocketBaseAuthService authService)
    {
        var token = await authService.ObtenerToken();

        var actualizado = await _productoService.ActualizarProducto(
            id,
            producto,
            token
        );

        if (actualizado == null)
        {
            return NotFound(new
            {
                mensaje = "No se pudo actualizar el producto o no existe"
            });
        }

        return Ok(actualizado);
    }


    // DELETE: api/productos/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> EliminarProducto(
        string id,
        [FromServices] PocketBaseAuthService authService)
    {
        var token = await authService.ObtenerToken();

        var eliminado = await _productoService.EliminarProducto(
            id,
            token
        );

        if (!eliminado)
        {
            return StatusCode(502, new
            {
                mensaje = "No se pudo eliminar el producto en PocketBase"
            });
        }

        return Ok(new
        {
            mensaje = "Producto eliminado correctamente",
            id = id
        });
    }


}


