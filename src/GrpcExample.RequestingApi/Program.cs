//using GrpcExample.
using Grpc.Core;
using Grpc.Net.Client;
using GrpcWeatherService;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

var app = builder.Build();

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", async () =>
{
    var channel = GrpcChannel.ForAddress("https://localhost:7201");
    var client = new WeatherService.WeatherServiceClient(channel);

    var request = new WeatherRequest();
    var response = await client.GetWeatherForecastAsync(request);

    //foreach (var forecast in response.Forecasts)
    //{
    //    Console.WriteLine($"Date: {forecast.Date}, Temp (C): {forecast.TemperatureC}, Summary: {forecast.Summary}, Temp (F): {forecast.TemperatureF}");
    //}

    return response.Forecasts;

    //var forecast = Enumerable.Range(1, 5).Select(index =>
    //    new WeatherForecast
    //    (
    //        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
    //        Random.Shared.Next(-20, 55),
    //        summaries[Random.Shared.Next(summaries.Length)]
    //    ))
    //    .ToArray();
    //return forecast;
});

app.MapGet("/weatherforecast/http", async () =>
{
    var client = new HttpClient();

    var response = await client.GetAsync("https://localhost:7201/weatherforecast");

    var forecasts = await response.Content.ReadFromJsonAsync<WeatherForecast[]>();

    return forecasts;
});

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
