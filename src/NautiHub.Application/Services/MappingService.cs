using AutoMapper;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Domain.Entities;

namespace NautiHub.Application.Services;

/// <summary>
/// Implementação do serviço de mapeamento usando AutoMapper.
/// </summary>
public class MappingService : IMappingService
{
    private readonly IMapper _mapper;

    /// <summary>
    /// Inicializa uma nova instância do serviço de mapeamento.
    /// </summary>
    /// <param name="mapper">Instância do AutoMapper.</param>
    public MappingService(IMapper mapper)
    {
        _mapper = mapper;
    }

    /// <inheritdoc/>
    public BoatResponse MapToBoatResponse(Boat boat)
    {
        return _mapper.Map<BoatResponse>(boat);
    }

    /// <inheritdoc/>
    public List<BoatListResponse> MapToBoatListResponse(List<Boat> boats)
    {
        return _mapper.Map<List<BoatListResponse>>(boats);
    }


}