
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using TiendaAPI.Models;

namespace TiendaAPI.Services;

public class ProductoService
{
    private readonly HttpClient _httpClient;

    private readonly JsonSerializerOptions _opciones = new()
    {
        PropertyNameCaseInsensitive = true
    };

    public ProductoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    // GET: Obtener todos los productos
    public async Task<List<Producto>> ObtenerProductos()
    {
        var respuesta = await _httpClient.GetAsync(
            "/api/collections/productos/records"
        );

        respuesta.EnsureSuccessStatusCode();

        var json = await respuesta.Content.ReadAsStringAsync();

        var resultado =
            JsonSerializer.Deserialize<RespuestaPocketBase>(
                json,
                _opciones
            );

        return resultado?.Items ?? new List<Producto>();
    }

    // GET: Obtener un producto por ID
    public async Task<Producto?> ObtenerProducto(string id)
    {
        var respuesta = await _httpClient.GetAsync(
            $"/api/collections/productos/records/{id}"
        );

        if (!respuesta.IsSuccessStatusCode)
            return null;

        return await respuesta.Content.ReadFromJsonAsync<Producto>(
            _opciones
        );
    }

    // POST: Crear un producto
    public async Task<Producto?> CrearProducto(
        CrearProductoRequest producto,
        string token)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Post,
            "/api/collections/productos/records"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        request.Content = JsonContent.Create(producto);

        var respuesta = await _httpClient.SendAsync(request);

        if (!respuesta.IsSuccessStatusCode)
            return null;

        return await respuesta.Content.ReadFromJsonAsync<Producto>(
            _opciones
        );
    }

    // PATCH: Actualizar un producto
    public async Task<Producto?> ActualizarProducto(
        string id,
        CrearProductoRequest producto,
        string token)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Patch,
            $"/api/collections/productos/records/{id}"
        );

        request.Headers.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        request.Content = JsonContent.Create(producto);

        var respuesta = await _httpClient.SendAsync(request);

        if (!respuesta.IsSuccessStatusCode)
            return null;

        return await respuesta.Content.ReadFromJsonAsync<Producto>(
            _opciones
        );
    }
// DELETE: Eliminar un producto
public async Task<bool> EliminarProducto(
    string id,
    string token)
{
    using var request = new HttpRequestMessage(
        HttpMethod.Delete,
        $"/api/collections/productos/records/{id}"
    );

    request.Headers.Authorization =
        new AuthenticationHeaderValue("Bearer", token);

    var respuesta = await _httpClient.SendAsync(request);

    if (!respuesta.IsSuccessStatusCode)
    {
        var error = await respuesta.Content.ReadAsStringAsync();

        throw new HttpRequestException(
            $"Error de PocketBase: {(int)respuesta.StatusCode} " +
            $"{respuesta.StatusCode}. Detalle: {error}"
        );
    }

    return true;
}

}

// Modelo para recibir la lista de productos de PocketBase
public class RespuestaPocketBase
{
    public List<Producto> Items { get; set; } = new();
}
