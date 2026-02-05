using NautiHub.Domain.Entities;
using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

public class BoatResponse
{
    // Identificação
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    // Tipo e especificações
    public BoatType Type { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public decimal Length { get; set; } // em metros
    public int MaxPassengers { get; set; }

    // Localização
    public string City { get; set; }
    public string State { get; set; }
    public string MarinaName { get; set; }
    public string DockNumber { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }

    // Preços
    public decimal PricePerDay { get; set; }
    public decimal? PricePerSeat { get; set; }
    public decimal SecurityDeposit { get; set; }

    // Status
    public bool IsActive { get; set; }
    public bool IsVerified { get; set; }
    public BoatStatus Status { get; set; }

    // Dono/Host
    public Guid HostId { get; set; }
    public User Host { get; set; }

    // Amenities
    public string AmenitiesJson { get; set; }

    // Relacionamentos
    public IList<string> Images { get; set; }

    public IList<Booking> Bookings { get; set; }

    public IList<Review> Reviews { get; set; }

    // Propriedades calculadas
    public decimal Rating { get; set; }

    public int ReviewCount { get; set; }

    public int ImageCount { get; set; }
}