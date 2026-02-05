using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Services.Ibge.Servers.Models.Responses;

public class CityIbgeResponse
{
    [JsonPropertyName("municipio-id")]
    public int Code { get; set; }

    [JsonPropertyName("municipio-nome")]
    public string Name { get; set; }

    [JsonPropertyName("UF-sigla")]
    public string Uf { get; set; }
}

