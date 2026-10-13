using TiendaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddHttpClient<ProductoService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PocketBase:BaseUrl"]!
    );
});

builder.Services.AddHttpClient<PocketBaseAuthService>(client =>
{
    client.BaseAddress = new Uri(
        builder.Configuration["PocketBase:BaseUrl"]!
    );
});

var app = builder.Build();

app.MapControllers();

app.Run();