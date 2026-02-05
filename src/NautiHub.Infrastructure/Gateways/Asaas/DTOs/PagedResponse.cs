using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace NautiHub.Infrastructure.Gateways.Asaas.DTOs;

/// <summary>
/// Resposta paginada da API Asaas
/// </summary>
public class PagedResponse<T>
{
    /// <summary>
    /// Dados da página atual
    /// </summary>
    [JsonPropertyName("data")]
    public List<T> Data { get; set; }

    /// <summary>
    /// Total de registros
    /// </summary>
    [JsonPropertyName("totalCount")]
    public int TotalCount { get; set; }

    /// <summary>
    /// Se tem mais dados
    /// </summary>
    [JsonPropertyName("hasMore")]
    public bool HasMore { get; set; }

    /// <summary>
    /// Limit solicitado
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; set; }

    /// <summary>
    /// Offset solicitado
    /// </summary>
    [JsonPropertyName("offset")]
    public int Offset { get; set; }
}