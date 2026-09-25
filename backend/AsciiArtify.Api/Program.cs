using AsciiArtify.Api;
using AsciiArtify.Api.Data;
using AsciiArtify.Api.Services;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
SQLitePCL.Batteries_V2.Init(); 
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=asciiart.db"));

builder.Services.AddCors(o => o.AddPolicy("AllowReact", p =>
    p.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();
app.Urls.Add("http://localhost:5225"); // fixed port so frontend can rely on it
app.UseCors("AllowReact");

using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.MapPost("/api/convert", async (IFormFile file, int? width, AppDbContext db) =>
{
    if (file is null || file.Length == 0)
        return Results.BadRequest("No file uploaded.");

    using var stream = file.OpenReadStream();
    using var image = await Image.LoadAsync<Rgba32>(stream);

    string ascii = AsciiConverter.Convert(image, width ?? 100);

    db.Conversions.Add(new ConversionRecord { FileName = file.FileName });
    await db.SaveChangesAsync();

    return Results.Ok(new { ascii });
}).DisableAntiforgery();

app.MapGet("/api/history", async (AppDbContext db) =>
    await db.Conversions.OrderByDescending(c => c.CreatedAt).Take(20).ToListAsync());

app.Run();