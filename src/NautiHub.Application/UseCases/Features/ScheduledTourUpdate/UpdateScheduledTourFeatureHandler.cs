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

namespace NautiHub.Application.UseCases.Features.ScheduledTourUpdate;

/// <summary>
/// Handler para atualização de passeio agendado
/// </summary>
public class UpdateScheduledTourFeatureHandler : FeatureHandler, IRequestHandler<UpdateScheduledTourFeature, FeatureResponse<ScheduledTourResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IScheduledTourRepository _scheduledTourRepository;
    private readonly ILogger<UpdateScheduledTourFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public UpdateScheduledTourFeatureHandler(
        DatabaseContext context,
        IScheduledTourRepository scheduledTourRepository,
        ILogger<UpdateScheduledTourFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _scheduledTourRepository = scheduledTourRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ScheduledTourResponse>> Handle(UpdateScheduledTourFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar passeio agendado
            var scheduledTour = await _context.Set<ScheduledTour>().FindAsync(request.TourId);
            if (scheduledTour == null)
            {
                _logger.LogWarning("Passeio agendado {TourId} não encontrado", request.TourId);
                AddError(_messagesService.ScheduledTour_Not_Found);
                return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Verificar se pode ser editado
            if (!scheduledTour.CanEdit)
            {
                _logger.LogWarning("Passeio agendado {TourId} não pode ser editado no status {Status}", 
                    request.TourId, scheduledTour.Status);
                AddError(_messagesService.ScheduledTour_Cannot_Edit_Status);
                return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Atualizar campos permitidos
            // Note: A entidade ScheduledTour não possui método público para atualizar a data
            // Esta validação pode precisar ser implementada na entidade ou via outra abordagem

            if (request.Data.StartTime.HasValue && request.Data.EndTime.HasValue)
            {
                // Verificar conflitos de horário se estiver atualizando
                if (request.Data.StartTime.Value >= request.Data.EndTime.Value)
                {
                    AddError(_messagesService.ScheduledTour_Start_After_End);
                    return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
                }

                var conflictingTours = await _scheduledTourRepository.GetConflictingToursAsync(
                    scheduledTour.BoatId,
                    request.Data.TourDate ?? scheduledTour.TourDate,
                    request.Data.StartTime.Value,
                    request.Data.EndTime.Value,
                    request.TourId);

                if (conflictingTours.Any())
                {
                    AddError(_messagesService.ScheduledTour_Conflict);
                    return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
                }

                scheduledTour.UpdateSchedule(request.Data.StartTime.Value, request.Data.EndTime.Value);
            }

            if (request.Data.AvailableSeats.HasValue)
                scheduledTour.UpdateAvailableSeats(request.Data.AvailableSeats.Value);

            if (request.Data.Notes != null)
                scheduledTour.UpdateNotes(request.Data.Notes);

            // Salvar no banco
            await _scheduledTourRepository.UpdateAsync(scheduledTour);
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

            _logger.LogInformation("Passeio agendado {TourId} atualizado com sucesso", scheduledTour.Id);

            return new FeatureResponse<ScheduledTourResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar passeio agendado {TourId}", request.TourId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ScheduledTourResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}