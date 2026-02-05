using Microsoft.Extensions.Logging;
using FluentValidation.Results;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Resources;
using NautiHub.Domain.Entities;
using NautiHub.Domain.Repositories;
using System.Net;
using MediatR;
using NautiHub.Infrastructure.Repositories;
using NautiHub.Infrastructure.DataContext;

namespace NautiHub.Application.UseCases.Features.ScheduledTourCreate;

/// <summary>
/// Handler para criação de passeio agendado
/// </summary>
public class CreateScheduledTourFeatureHandler : FeatureHandler, IRequestHandler<CreateScheduledTourFeature, FeatureResponse<ScheduledTourResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IScheduledTourRepository _scheduledTourRepository;
    private readonly IBoatRepository _boatRepository;
    private readonly ILogger<CreateScheduledTourFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public CreateScheduledTourFeatureHandler(
        DatabaseContext context,
        IScheduledTourRepository scheduledTourRepository,
        IBoatRepository boatRepository,
        ILogger<CreateScheduledTourFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _scheduledTourRepository = scheduledTourRepository;
        _boatRepository = boatRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ScheduledTourResponse>> Handle(CreateScheduledTourFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Validar se o barco existe e está ativo
            var boat = await _context.Set<Boat>().FindAsync(request.Data.BoatId);
            if (boat == null)
            {
                _logger.LogWarning("Barco {BoatId} não encontrado", request.Data.BoatId);
                AddError(_messagesService.Boat_Not_Found);
                return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Verificar conflitos de horário
            var conflictingTours = await _scheduledTourRepository.GetConflictingToursAsync(
                request.Data.BoatId,
                request.Data.TourDate,
                request.Data.StartTime,
                request.Data.EndTime);

            if (conflictingTours.Any())
            {
                _logger.LogWarning("Conflito de horário encontrado para barco {BoatId} na data {Date}", 
                    request.Data.BoatId, request.Data.TourDate);
                AddError(_messagesService.ScheduledTour_Conflict);
                return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Criar passeio agendado
            var scheduledTour = new ScheduledTour(
                boat.UserId, // BoatOwnerId
                request.Data.BoatId,
                request.Data.TourDate,
                request.Data.StartTime,
                request.Data.EndTime,
                request.Data.AvailableSeats,
                request.Data.Status,
                request.Data.Notes);

            // Salvar no banco
            await _scheduledTourRepository.AddAsync(scheduledTour);
            await _context.SaveChangesAsync();

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

            _logger.LogInformation("Passeio agendado {TourId} criado com sucesso para barco {BoatId}", 
                scheduledTour.Id, scheduledTour.BoatId);

            return new FeatureResponse<ScheduledTourResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar passeio agendado para barco {BoatId}", request.Data.BoatId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}