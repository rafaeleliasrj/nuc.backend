using MediatR;
using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Communication;
using NautiHub.Core.Messages.Queries;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Domain.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Application.UseCases.Queries.ScheduledTourByBoatId;

/// <summary>
/// Handler para buscar passeios agendados por ID do barco
/// </summary>
public class GetScheduledTourByBoatIdQueryHandler(
    DatabaseContext context,
    IScheduledTourRepository scheduledTourRepository,
    ILogger<GetScheduledTourByBoatIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetScheduledTourByBoatIdQuery, QueryResponse<ScheduledTourListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IScheduledTourRepository _scheduledTourRepository = scheduledTourRepository;
    private readonly ILogger<GetScheduledTourByBoatIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ScheduledTourListResponse>> Handle(GetScheduledTourByBoatIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar passeios por ID do barco
            var items = await _scheduledTourRepository.GetByBoatIdAsync(request.BoatId);
            
            // Aplicar paginação
            var total = items.Count();
            var pagedItems = items
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Mapear para response
            var tours = pagedItems.Select(scheduledTour => new ScheduledTourResponse
            {
                Id = scheduledTour.Id,
                UserId = scheduledTour.UserId,
                BoatId = scheduledTour.BoatId,
                TourDate = scheduledTour.TourDate,
                StartTime = scheduledTour.StartTime,
                EndTime = scheduledTour.EndTime,
                AvailableSeats = scheduledTour.AvailableSeats,
                Status = scheduledTour.Status,
                Notes = scheduledTour.Notes,
                IsActive = scheduledTour.IsActive,
                CanEdit = scheduledTour.CanEdit,
                CreatedAt = scheduledTour.CreatedAt!.Value,
                UpdatedAt = scheduledTour.UpdatedAt
            }).ToList();

            var response = new ScheduledTourListResponse
            {
                ScheduledTours = tours,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                Filters = new ScheduledTourFilters
                {
                    BoatId = request.BoatId
                }
            };

            return new QueryResponse<ScheduledTourListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar passeios para barco {BoatId}", request.BoatId);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("BoatId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ScheduledTourListResponse>(validationResult);
        }
    }
}