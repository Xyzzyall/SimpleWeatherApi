using Microsoft.AspNetCore.Mvc;
using SimpleWeatherApi.ApiClients.OpenWeather;
using SimpleWeatherApi.ApiClients.OpenWeather.ApiModels;
using SimpleWeatherApi.Models;

namespace SimpleWeatherApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;
    private readonly IOpenWeatherClient _openWeatherClient;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, IOpenWeatherClient openWeatherClient)
    {
        _logger = logger;
        _openWeatherClient = openWeatherClient;
    }

    [HttpGet("OpenWeather", Name = "OpenWeatherForecast")]
    public async Task<Forecast> GetForecast(string city, CancellationToken ct)
    {
        var cords = (await _openWeatherClient.GetCityCordsAsync(city, ct)).First();
        return await _openWeatherClient.GetForecastAsync(cords.Lon, cords.Lat, ct);
    }
    
    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get(CancellationToken ct)
    {
        var geo = await _openWeatherClient.GetCityCordsAsync("Moscow", ct: ct);
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
    }
}