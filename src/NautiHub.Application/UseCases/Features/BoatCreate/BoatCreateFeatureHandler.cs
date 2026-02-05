using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.DomainObjects;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;
using System.Net;

namespace NautiHub.Application.UseCases.Features.CadastrarEmpresa;

public class BoatCreateFeatureHandler(
    DatabaseContext context,
    ILogger<BoatCreateFeatureHandler> logger,
    IBoatRepository boatRepository,
    INautiHubIdentity nautiHubIdentity,
    MessagesService messagesService
) : FeatureHandler(context), IRequestHandler<BoatCreateFeature, FeatureResponse<Guid>>
{
    private readonly ILogger<BoatCreateFeatureHandler> _logger = logger;
    private readonly IBoatRepository _boatRepository = boatRepository;
    private readonly INautiHubIdentity _nautiHubIdentity = nautiHubIdentity;
    private readonly MessagesService _messagesService = messagesService;


    public async Task<FeatureResponse<Guid>> Handle(
        BoatCreateFeature request,
        CancellationToken cancellationToken
    )
    {
        var exist = await _boatRepository.ExistAsync();

        if (exist)
        {
            AddError(_messagesService.Boat_Already_Exists);
            return new FeatureResponse<Guid>(ValidationResult, _nautiHubIdentity.UserId!.Value, HttpStatusCode.Conflict);
        }

        var boat = new Boat(
                    request.Data.UserId,
                    request.Data.Name,
                    request.Data.Description,
                    request.Data.Document,
                    request.Data.Capacity,
                    request.Data.PricePerPerson,
                    request.Data.LocationCity,
                    request.Data.LocationState,
                    request.Data.BoatType
                    );

        await _boatRepository.AddAsync(boat);

        await SaveChanges();

        return new FeatureResponse<Guid>(ValidationResult, boat.Id);
    }
}
