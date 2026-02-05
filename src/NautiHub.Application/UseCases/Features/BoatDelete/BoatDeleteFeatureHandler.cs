using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using System.Net;

namespace NautiHub.Application.UseCases.Features.ExcluirEmpresa;

public class BoatDeleteFeatureHandler(
    DatabaseContext context,
    ILogger<BoatDeleteFeatureHandler> logger,
    IBoatRepository boatRepository,
    MessagesService messagesService
) : FeatureHandler(context), IRequestHandler<BoatDeleteFeature, FeatureResponse<bool>>
{
    private readonly ILogger<BoatDeleteFeatureHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<bool>> Handle(
        BoatDeleteFeature request,
        CancellationToken cancellationToken
    )
    {
        Boat? boat = await _boatRepository.GetByIdAsync(
            request.BoatId
        );

        if (boat is null)
        {
            AddError(_messagesService.Boat_Not_Found);
            return new FeatureResponse<bool>(ValidationResult, false, HttpStatusCode.NotFound);
        }

        await _boatRepository.DeleteAsync(boat);

        await SaveChanges();

        return new FeatureResponse<bool>(ValidationResult, true);
    }
}
