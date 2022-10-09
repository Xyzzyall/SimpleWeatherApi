using SimpleWeatherApi.ApiClients.OpenWeather.ApiModels;

namespace SimpleWeatherApi.ApiClients.OpenWeather;

public interface IOpenWeatherClient
{
    Task<Forecast> GetForecastAsync(decimal lon, decimal lat, CancellationToken ct = default);
    Task<GeoCoderResponse[]> GetCityCordsAsync(string query, CancellationToken ct = default);
}