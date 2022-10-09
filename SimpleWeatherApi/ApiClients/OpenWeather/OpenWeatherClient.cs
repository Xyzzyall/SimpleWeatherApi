using SimpleWeatherApi.ApiClients.OpenWeather.ApiModels;

namespace SimpleWeatherApi.ApiClients.OpenWeather;

public class OpenWeatherClient: IOpenWeatherClient
{
    private readonly HttpClient _client;
    private readonly OpenWeatherApiSettings _settings;
    public OpenWeatherClient(HttpClient client, IConfiguration configs)
    {
        _client = client;
        _settings = configs.GetSection("OpenWeatherApi").Get<OpenWeatherApiSettings>();
    }

    public async Task<Forecast> GetForecastAsync(decimal lon, decimal lat, CancellationToken ct = default)
    {
        var forecast = await _client.GetFromJsonAsync<Forecast>(
            $"data/2.5/weather?lat={lat}&lon={lon}&units=metric&appid={_settings.ApiKey}",
            ct
        );
        return forecast!;
    }

    public async Task<GeoCoderResponse[]> GetCityCordsAsync(string query, CancellationToken ct = default)
    {
        var geoCoderResponses = await _client.GetFromJsonAsync<GeoCoderResponse[]>(
            $"geo/1.0/direct?q={query}&limit={1}&appid={_settings.ApiKey}", 
            cancellationToken: ct
        );
        return geoCoderResponses ?? Array.Empty<GeoCoderResponse>();
    }
}