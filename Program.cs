using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.WebHost.UseUrls("http://localhost:5000");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                     ?? "server=db;database=nucdb;user=root;password=example";
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapPost("/weatherforecast", async (AppDbContext db) =>
{
    var summaries = new[]
    {
        "Congelante","Gelado","Frio","Fresco","Ameno","Quente","Agradável","Quente","Escaldante","Ardente"
    };

    var forecast = new WeatherForecastEntity
    {
        Date = DateOnly.FromDateTime(DateTime.Now),
        TemperatureC = Random.Shared.Next(-20, 55),
        Summary = summaries[Random.Shared.Next(summaries.Length)]
    };

    db.WeatherForecasts.Add(forecast);
    await db.SaveChangesAsync();

    return forecast;
});

app.MapGet("/weatherforecast", async (AppDbContext db) =>
{
    var forecasts = await db.WeatherForecasts.ToArrayAsync();
    return forecasts;
})
.WithName("GetWeatherForecast");

app.Run();
