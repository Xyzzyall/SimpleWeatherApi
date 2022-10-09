using LazyCache;
using SimpleWeatherApi.ApiClients.OpenWeather.ApiModels;

namespace SimpleWeatherApi.ApiClients.OpenWeather;

public class CachedOpenWeatherClient : IOpenWeatherClient
{
    private readonly IOpenWeatherClient _client;
    private readonly IAppCache _cache;

    public CachedOpenWeatherClient(IOpenWeatherClient client, IAppCache cache)
    {
        _client = client;
        _cache = cache;
    }

    public async Task<Forecast> GetForecastAsync(decimal lon, decimal lat, CancellationToken ct = default)
    {
        return await _cache.GetOrAddAsync(
            $"OpenWeatherApi-Forecast-{lon}-{lat}", 
            async () => await _client.GetForecastAsync(lon, lat, ct),
            TimeSpan.FromMinutes(15)
        );
    }

    public async Task<GeoCoderResponse[]> GetCityCordsAsync(string query, CancellationToken ct = default)
    {
        return await _cache.GetOrAddAsync(
            $"OpenWeatherApi-CityCords-{query}", 
            async () => await _client.GetCityCordsAsync(query, ct),
            TimeSpan.FromHours(24)
        );
    }
}