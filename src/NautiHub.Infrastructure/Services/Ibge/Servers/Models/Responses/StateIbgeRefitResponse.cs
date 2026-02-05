using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Services.Ibge.Servers.Models.Responses;

public class StateIbgeResponse
{
    [JsonPropertyName("UF-id")]
    public int Code { get; set; }

    [JsonPropertyName("UF-nome")]
    public string Name { get; set; }

    [JsonPropertyName("UF-sigla")]
    public string Uf { get; set; }
}
