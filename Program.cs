using Microsoft.EntityFrameworkCore;
using TiendaAPI.Data;
using TiendaAPI.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler =
            System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    });

// Conexión con PostgreSQL mediante Entity Framework Core
builder.Services.AddDbContext<TiendaDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("TiendaConnection")));

// Servicios antiguos de PocketBase.
// Los retiraremos cuando el nuevo CRUD esté funcionando.
// builder.Services.AddHttpClient<ProductoService>(client =>
// {
//     client.BaseAddress = new Uri(
//         builder.Configuration["PocketBase:BaseUrl"]!
//     );
// });

// builder.Services.AddHttpClient<PocketBaseAuthService>(client =>
// {
//     client.BaseAddress = new Uri(
//         builder.Configuration["PocketBase:BaseUrl"]!
//     );
// });

var app = builder.Build();

app.MapControllers();

app.Run();
