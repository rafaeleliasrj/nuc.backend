using NautiHub.Core.DomainObjects;
using NautiHub.Domain.Enums;
using NautiHub.Domain.Exceptions;

namespace NautiHub.Domain.Entities;

/// <summary>
/// Entidade que representa uma embarcação no sistema de aluguel.
/// </summary>
public class Boat : Entity, IAggregateRoot
{
    /// <summary>
    /// Construtor padrão para EF Core.
    /// </summary>
    private Boat() { }

    /// <summary>
    /// Construtor com dados iniciais da embarcação.
    /// </summary>
    /// <param name="userId">Identificador do usuário proprietário.</param>
    /// <param name="name">Nome da embarcação.</param>
    /// <param name="description">Descrição detalhada da embarcação.</param>
    /// <param name="document">Documento de registro da embarcação.</param>
    /// <param name="capacity">Capacidade máxima de passageiros.</param>
    /// <param name="pricePerPerson">Preço por pessoa.</param>
    /// <param name="locationCity">Cidade da localização.</param>
    /// <param name="locationState">Estado da localização.</param>
    /// <param name="boatType">Tipo da embarcação.</param>
    /// <param name="boatStatus">Status de aprovação da embarcação.</param>
    public Boat(
        Guid userId,
        string name,
        string description,
        string document,
        int capacity,
        decimal pricePerPerson,
        string locationCity,
        string locationState,
        BoatType boatType,
        BoatStatus boatStatus = BoatStatus.Pending)
    {
        UserId = userId;
        Name = name;
        Description = description;
        Document = document;
        Capacity = capacity;
        PricePerPerson = pricePerPerson;
        LocationCity = locationCity;
        LocationState = locationState;
        BoatType = boatType;
        Status = boatStatus;
        IsActive = false;
        
        Validate();
    }

    /// <summary>
    /// Identificador do usduário
    /// </summary>
    public Guid UserId { get; private set; }

    /// <summary>
    /// Relacionamento com o usuário
    /// </summary>
    public User User { get; private set; }

    /// <summary>
    /// Nome da embarcação.
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// Descrição detalhada da embarcação.
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// Documento de registro da embarcação.
    /// </summary>
    public string Document { get; private set; }

    /// <summary>
    /// Capacidade máxima de passageiros.
    /// </summary>
    public int Capacity { get; private set; }

    /// <summary>
    /// Preço por pessoa.
    /// </summary>
    public decimal PricePerPerson { get; private set; }

    /// <summary>
    /// URL da imagem principal da embarcação.
    /// </summary>
    public string? ImageUrl { get; private set; }

    /// <summary>
    /// Cidade da localização da embarcação.
    /// </summary>
    public string LocationCity { get; private set; } = string.Empty;

    /// <summary>
    /// Estado da localização da embarcação.
    /// </summary>
    public string LocationState { get; private set; } = string.Empty;

    /// <summary>
    /// Tipo da embarcação.
    /// </summary>
    public BoatType BoatType { get; private set; }

    /// <summary>
    /// Comodidades da embarcação.
    /// </summary>
    public string? Amenities { get; private set; }

    /// <summary>
    /// URL do documento da embarcação.
    /// </summary>
    public string? DocumentUrl { get; private set; }

    /// <summary>
    /// Coleção de imagens da embarcação.
    /// </summary>
    public virtual ICollection<string> Images { get; private set; } = new List<string>();

    /// <summary>
    /// Status de aprovação do cadastro da embarcação.
    /// </summary>
    public BoatStatus Status { get; private set; }

    /// <summary>
    /// Indica se a embarcação está ativa no sistema.
    /// </summary>
    public bool IsActive { get; private set; }

    /// <summary>
    /// Atualiza os dados básicos da embarcação.
    /// </summary>
    /// <param name="name">Novo nome.</param>
    /// <param name="description">Nova descrição.</param>
    /// <param name="capacity">Nova capacidade.</param>
    /// <param name="pricePerPerson">Novo preço por pessoa.</param>
    /// <param name="locationCity">Nova cidade.</param>
    /// <param name="locationState">Novo estado.</param>
    /// <param name="boatType">Novo tipo.</param>
    public void UpdateBasicInfo(
        string name,
        string description,
        int capacity,
        decimal pricePerPerson,
        string locationCity,
        string locationState,
        BoatType boatType)
    {
        Name = name;
        Description = description;
        Capacity = capacity;
        PricePerPerson = pricePerPerson;
        LocationCity = locationCity;
        LocationState = locationState;
        BoatType = boatType;
        
        Validate();
    }

    /// <summary>
    /// Atualiza a imagem principal da embarcação.
    /// </summary>
    /// <param name="imageUrl">URL da nova imagem principal.</param>
    public void UpdateImageUrl(string? imageUrl)
    {
        ImageUrl = imageUrl;
    }

    /// <summary>
    /// Atualiza as comodidades da embarcação.
    /// </summary>
    /// <param name="amenities">Novas comodidades.</param>
    public void UpdateAmenities(string? amenities)
    {
        Amenities = amenities;
    }

    /// <summary>
    /// Atualiza o documento da embarcação.
    /// </summary>
    /// <param name="document">Novo documento.</param>
    /// <param name="documentUrl">URL do novo documento.</param>
    public void UpdateDocument(string document, string? documentUrl)
    {
        Document = document;
        DocumentUrl = documentUrl;
    }

    /// <summary>
    /// Adiciona uma imagem à coleção de imagens.
    /// </summary>
    /// <param name="imageUrl">URL da imagem a ser adicionada.</param>
    public void AddImage(string imageUrl)
    {
        if (!string.IsNullOrWhiteSpace(imageUrl) && !Images.Contains(imageUrl))
        {
            Images.Add(imageUrl);
        }
    }

    /// <summary>
    /// Remove uma imagem da coleção de imagens.
    /// </summary>
    /// <param name="imageUrl">URL da imagem a ser removida.</param>
    public void RemoveImage(string imageUrl)
    {
        Images.Remove(imageUrl);
    }

    /// <summary>
    /// Ativa a embarcação após aprovação.
    /// </summary>
    public void Activate()
    {
        if (Status != BoatStatus.Approved)
            throw BoatDomainException.OnlyApprovedCanBeActive();
        
        IsActive = true;
    }

    /// <summary>
    /// Desativa a embarcação.
    /// </summary>
    public void Deactivate()
    {
        IsActive = false;
    }

    /// <summary>
    /// Aprova o cadastro da embarcação.
    /// </summary>
    public void Approve()
    {
        if (Status == BoatStatus.Approved)
            throw BoatDomainException.AlreadyApproved();
        
        Status = BoatStatus.Approved;
    }

    /// <summary>
    /// Rejeita o cadastro da embarcação.
    /// </summary>
    /// <param name="reason">Motivo da rejeição.</param>
    public void Reject(string reason)
    {
        if (Status == BoatStatus.Approved)
            throw BoatDomainException.CannotRejectApproved();
        
        Status = BoatStatus.Rejected;
        IsActive = false;
    }

    /// <summary>
    /// Suspende temporariamente a embarcação.
    /// </summary>
    public void Suspend()
    {
        if (Status != BoatStatus.Approved)
            throw BoatDomainException.OnlyApprovedCanBeSuspended();
        
        Status = BoatStatus.Suspended;
        IsActive = false;
    }

    /// <summary>
    /// Cancela o cadastro da embarcação.
    /// </summary>
    public void Cancel()
    {
        Status = BoatStatus.Cancelled;
        IsActive = false;
    }

    /// <summary>
    /// Reativa uma embarcação suspensa.
    /// </summary>
    public void Reactivate()
    {
        if (Status != BoatStatus.Suspended)
            throw BoatDomainException.OnlySuspendedCanBeReactived();
        
        Status = BoatStatus.Approved;
        IsActive = true;
    }

    /// <summary>
    /// Valida as regras de negócio da embarcação.
    /// </summary>
    private void Validate()
    {
        if (string.IsNullOrWhiteSpace(Name))
            throw BoatDomainException.NameRequired();

        if (Name.Length > 200)
            throw BoatDomainException.NameTooLong();

        if (string.IsNullOrWhiteSpace(Description))
            throw BoatDomainException.DescriptionRequired();

        if (Description.Length > 2000)
            throw BoatDomainException.DescriptionTooLong();

        if (string.IsNullOrWhiteSpace(Document))
            throw BoatDomainException.DocumentRequired();

        if (Document.Length > 50)
            throw BoatDomainException.DocumentTooLong();

        if (Capacity <= 0)
            throw BoatDomainException.CapacityInvalid();

        if (Capacity > 1000)
            throw BoatDomainException.CapacityTooHigh();

        if (PricePerPerson <= 0)
            throw BoatDomainException.PriceInvalid();

        if (PricePerPerson > 100000)
            throw BoatDomainException.PriceTooHigh();

        if (string.IsNullOrWhiteSpace(LocationCity))
            throw BoatDomainException.CityRequired();

        if (LocationCity.Length > 100)
            throw BoatDomainException.CityTooLong();

        if (string.IsNullOrWhiteSpace(LocationState))
            throw BoatDomainException.StateRequired();

        if (LocationState.Length != 2)
            throw BoatDomainException.StateInvalidFormat();
    }
}