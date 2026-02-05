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

namespace NautiHub.Application.UseCases.Features.ScheduledTourDelete;

/// <summary>
/// Handler para exclusão de passeio agendado
/// </summary>
public class DeleteScheduledTourFeatureHandler : FeatureHandler, IRequestHandler<DeleteScheduledTourFeature, FeatureResponse<bool>>
{
    private readonly DatabaseContext _context;
    private readonly IScheduledTourRepository _scheduledTourRepository;
    private readonly ILogger<DeleteScheduledTourFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public DeleteScheduledTourFeatureHandler(
        DatabaseContext context,
        IScheduledTourRepository scheduledTourRepository,
        ILogger<DeleteScheduledTourFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _scheduledTourRepository = scheduledTourRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<bool>> Handle(DeleteScheduledTourFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar passeio agendado
            var scheduledTour = await _context.Set<ScheduledTour>().FindAsync(request.TourId);
            if (scheduledTour == null)
            {
                _logger.LogWarning("Passeio agendado {TourId} não encontrado", request.TourId);
                AddError(_messagesService.ScheduledTour_Not_Found);
                return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Verificar se pode ser cancelado
            if (scheduledTour.Status == Domain.Enums.ScheduledTourStatus.Completed)
            {
                _logger.LogWarning("Passeio agendado {TourId} não pode ser excluído pois já foi concluído", request.TourId);
                AddError(_messagesService.ScheduledTour_Cannot_Delete_Completed);
                return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Cancelar o passeio (não delete físico, apenas mudança de status)
            scheduledTour.Cancel();

            // Salvar no banco
            await _scheduledTourRepository.UpdateAsync(scheduledTour);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Passeio agendado {TourId} cancelado com sucesso", scheduledTour.Id);

            return new FeatureResponse<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar passeio agendado {TourId}", request.TourId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}