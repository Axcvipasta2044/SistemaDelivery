using System.Net.Http.Json;
using TiendaAPI.Models;

namespace TiendaAPI.Services;

public class PocketBaseAuthService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public PocketBaseAuthService(
        HttpClient httpClient,
        IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<string> ObtenerToken()
    {
        var email = _configuration["PocketBase:AdminEmail"];
        var password = _configuration["PocketBase:AdminPassword"];

        var datos = new
        {
            identity = email,
            password = password
        };

        var respuesta = await _httpClient.PostAsJsonAsync(
            "/api/collections/_superusers/auth-with-password",
            datos
        );

        respuesta.EnsureSuccessStatusCode();

        var resultado = await respuesta.Content
            .ReadFromJsonAsync<PocketBaseAuthResponse>();

        return resultado!.Token;
    }


}

public class PocketBaseAuthResponse
{
    public string Token { get; set; } = "";
}



