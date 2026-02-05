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

namespace NautiHub.Application.UseCases.Queries.ScheduledTourById;

/// <summary>
/// Handler para buscar passeio agendado por ID
/// </summary>
public class GetScheduledTourByIdQueryHandler(
    DatabaseContext context,
    IScheduledTourRepository scheduledTourRepository,
    ILogger<GetScheduledTourByIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetScheduledTourByIdQuery, QueryResponse<ScheduledTourResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IScheduledTourRepository _scheduledTourRepository = scheduledTourRepository;
    private readonly ILogger<GetScheduledTourByIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ScheduledTourResponse>> Handle(GetScheduledTourByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar passeio agendado
            var scheduledTour = await _context.Set<ScheduledTour>().FindAsync(request.Id);
            if (scheduledTour == null)
            {
                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationFailure("TourId", _messagesService.ScheduledTour_Not_Found));
                return new QueryResponse<ScheduledTourResponse>(validationResult);
            }

            // Mapear para response
            var response = new ScheduledTourResponse
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
            };

            return new QueryResponse<ScheduledTourResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar passeio agendado {TourId}", request.Id);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("TourId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ScheduledTourResponse>(validationResult);
        }
    }
}