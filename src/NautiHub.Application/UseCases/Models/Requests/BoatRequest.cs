using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Requests;

public class BoatRequest
{
    /// <summary>
    /// Identificador do usduário
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Relacionamento com o usuário
    /// </summary>
    public User User { get; set; }

    /// <summary>
    /// Nome da embarcação.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Descrição detalhada da embarcação.
    /// </summary>
    public string Description { get; set; }

    /// <summary>
    /// Documento de registro da embarcação.
    /// </summary>
    public string Document { get; set; }

    /// <summary>
    /// Capacidade máxima de passageiros.
    /// </summary>
    public int Capacity { get; set; }

    /// <summary>
    /// Preço por pessoa.
    /// </summary>
    public decimal PricePerPerson { get; set; }

    /// <summary>
    /// URL da imagem principal da embarcação.
    /// </summary>
    public string? ImageUrl { get; set; }

    /// <summary>
    /// Cidade da localização da embarcação.
    /// </summary>
    public string LocationCity { get; set; } = string.Empty;

    /// <summary>
    /// Estado da localização da embarcação.
    /// </summary>
    public string LocationState { get; set; } = string.Empty;

    /// <summary>
    /// Tipo da embarcação.
    /// </summary>
    public BoatType BoatType { get; set; }

    /// <summary>
    /// Comodidades da embarcação.
    /// </summary>
    public string? Amenities { get; set; }

    /// <summary>
    /// URL do documento da embarcação.
    /// </summary>
    public string? DocumentUrl { get; set; }

    /// <summary>
    /// Coleção de imagens da embarcação.
    /// </summary>
    public virtual ICollection<string> Images { get; set; } = new List<string>();

    /// <summary>
    /// Status de aprovação do cadastro da embarcação.
    /// </summary>
    public BoatStatus Status { get; set; }

    /// <summary>
    /// Indica se a embarcação está ativa no sistema.
    /// </summary>
    public bool IsActive { get; set; }
}