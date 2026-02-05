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

namespace NautiHub.Application.UseCases.Queries.ScheduledTourList;

/// <summary>
/// Handler para listar passeios agendados
/// </summary>
public class GetScheduledTourListQueryHandler(
    DatabaseContext context,
    IScheduledTourRepository scheduledTourRepository,
    ILogger<GetScheduledTourListQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetScheduledTourListQuery, QueryResponse<ScheduledTourListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IScheduledTourRepository _scheduledTourRepository = scheduledTourRepository;
    private readonly ILogger<GetScheduledTourListQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ScheduledTourListResponse>> Handle(GetScheduledTourListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Listar passeios com paginação e filtros
            var (items, total) = await _scheduledTourRepository.ListAsync(
                page: request.Page,
                perPage: request.PageSize,
                search: null,
                boatId: request.BoatId,
                boatOwnerId: null,
                status: request.Status,
                date: null,
                dateStart: request.StartDate,
                dateEnd: request.EndDate,
                minPrice: null,
                maxPrice: null,
                orderBy: null);

            // Filtrar por IsActive se especificado
            if (request.IsActive.HasValue)
            {
                items = items.Where(t => t.IsActive == request.IsActive.Value);
            }

            // Filtrar por MinAvailableSeats se especificado
            if (request.MinAvailableSeats.HasValue)
            {
                items = items.Where(t => t.AvailableSeats >= request.MinAvailableSeats.Value);
            }

            // Aplicar paginação após filtros
            items = items.Skip((request.Page - 1) * request.PageSize).Take(request.PageSize);

            // Mapear para response
            var tours = items.Select(scheduledTour => new ScheduledTourResponse
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
                    BoatId = request.BoatId,
                    Status = request.Status,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    IsActive = request.IsActive,
                    MinAvailableSeats = request.MinAvailableSeats
                }
            };

            return new QueryResponse<ScheduledTourListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar passeios agendados");
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("List", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ScheduledTourListResponse>(validationResult);
        }
    }
}