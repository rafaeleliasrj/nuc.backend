using AutoMapper;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Domain.Entities;

namespace NautiHub.Application.Services;

/// <summary>
/// Interface de serviço para mapeamento de entidades usando AutoMapper.
/// </summary>
public interface IMappingService
{
    /// <summary>
    /// Mapeia uma entidade Boat para BoatResponse.
    /// </summary>
    /// <param name="boat">Entidade Boat de origem.</param>
    /// <returns>BoatResponse mapeado.</returns>
    BoatResponse MapToBoatResponse(Boat boat);

    /// <summary>
    /// Mapeia uma lista de entidades Boat para BoatListResponse.
    /// </summary>
    /// <param name="boats">Lista de entidades Boat de origem.</param>
    /// <returns>Lista de BoatListResponse mapeada.</returns>
    List<BoatListResponse> MapToBoatListResponse(List<Boat> boats);


}