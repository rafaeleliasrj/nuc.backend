using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Domain.Services.InfrastructureService.Cache;
using NautiHub.Infrastructure.DataContext;
using System.Net;

namespace NautiHub.Application.UseCases.Features.ExcluirLogo;

public class BoatImageDeleteFeatureHandler(
    DatabaseContext context,
    ILogger<BoatImageDeleteFeatureHandler> logger,
    IBoatRepository boatRepository,
    ICacheService cacheService,
    INautiHubIdentity nautiHubIdentity,
    MessagesService messagesService
    )
        : FeatureHandler(context),
        IRequestHandler<BoatImageDeleteFeature, FeatureResponse<bool>>
{
    private readonly ILogger<BoatImageDeleteFeatureHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly ICacheService _cacheService = cacheService;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubIdentity;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<FeatureResponse<bool>> Handle(
        BoatImageDeleteFeature request,
        CancellationToken cancellationToken
    )
    {
        Boat? boat = await _boatRepository.GetByIdAsync(request.BoatId);

        if (boat == null)
            return new FeatureResponse<bool>(ValidationResult);

        if (boat.Images is null)
        {
            AddError(_messagesService.BoatImage_Not_Found);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
        }

        //TODO: Implementar exclusão de imagem
        //boat.RemoveImage(request.BoatId);

        await _boatRepository.UpdateAsync(boat);
        await SaveChanges();

        var cacheKey = $"boat:{_nautiHubIdentity.UserId}:{request.BoatId}";
        await _cacheService.DeleteAsync(cacheKey);

        return new FeatureResponse<bool>(ValidationResult, true);
    }
}
