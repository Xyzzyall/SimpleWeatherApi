using System.Text.Json.Serialization;

namespace SimpleWeatherApi.ApiClients.OpenWeather.ApiModels;

public record GeoCoderResponse(
    [property: JsonPropertyName("name")] string Name,
    [property: JsonPropertyName("local_names")] IReadOnlyDictionary<string, string> LocalNames,
    [property: JsonPropertyName("lat")] decimal Lat,
    [property: JsonPropertyName("lon")] decimal Lon,
    [property: JsonPropertyName("country")] string Country,
    [property: JsonPropertyName("state")] string State
);

