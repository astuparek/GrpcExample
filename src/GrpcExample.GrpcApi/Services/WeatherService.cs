using Grpc.Core;
using GrpcWeatherService;

namespace GrpcExample.GrpcApi.Services;

public class WeatherServiceImpl : WeatherService.WeatherServiceBase
{
    public override Task<WeatherResponse> GetWeatherForecast(WeatherRequest request, ServerCallContext context)
    {
        var summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        var forecast = Enumerable.Range(1, 5).Select(index =>
            new GrpcWeatherService.WeatherForecast {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)).ToString("yyyy-MM-dd"),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = summaries[Random.Shared.Next(summaries.Length)]
            }
        ).ToArray();

        var response = new WeatherResponse();
        response.Forecasts.AddRange(forecast);

        return Task.FromResult(response);
    }
}
