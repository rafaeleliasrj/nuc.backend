using NautiHub.Domain.Enums;

namespace NautiHub.Application.UseCases.Models.Responses;

public class BoatListResponse
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public BoatType Type { get; set; }
    public string Model { get; set; }
}