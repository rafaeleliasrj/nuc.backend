using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Application.UseCases.Features.ScheduledTourCreate;
using NautiHub.Application.UseCases.Features.ScheduledTourUpdate;

using NautiHub.Application.UseCases.Features.ScheduledTourDelete;
using NautiHub.Application.UseCases.Queries.ScheduledTourById;
using NautiHub.Application.UseCases.Queries.ScheduledTourList;
using NautiHub.Application.UseCases.Queries.ScheduledTourByBoatId;
using NautiHub.Core.Controllers;
using NautiHub.Core.Mediator;
using NautiHub.Domain.Enums;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para gerenciamento de passeios agendados
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ScheduledToursController(IMediatorHandler mediator, MessagesService messagesService) : MainController(mediator, messagesService)
{
    private readonly IMediatorHandler _mediator = mediator;

    /// <summary>
    /// Criar novo passeio agendado
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateScheduledTourRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new CreateScheduledTourFeature
        {
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Atualizar passeio agendado
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateScheduledTourRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new UpdateScheduledTourFeature
        {
            TourId = id,
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Atualizar status do passeio agendado
    /// </summary>
    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> UpdateStatusAsync([FromRoute] Guid id, [FromBody] UpdateScheduledTourStatusRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        // For now, remove the status update endpoint since Status is not available in UpdateScheduledTourRequest
        // This would need to be implemented as a separate feature
        return BadRequest(_messagesService?.ScheduledTour_Status_Not_Implemented ?? "Atualização de status não implementada.");
    }

    /// <summary>
    /// Excluir passeio agendado
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _mediator.ExecuteFeature(new DeleteScheduledTourFeature
        {
            TourId = id
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar passeio agendado por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var query = new GetScheduledTourByIdQuery(id);
        var result = await _mediator.ExecuteQuery<ScheduledTourResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        if (result.Response == null)
            return NotFound(_messagesService?.Error_Record_Not_Found ?? "Registro não encontrado.");

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Listar passeios agendados
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListAsync(
        [FromQuery] Guid? boatId = null, 
        [FromQuery] ScheduledTourStatus? status = null, 
        [FromQuery] DateOnly? startDate = null, 
        [FromQuery] DateOnly? endDate = null, 
        [FromQuery] bool? isActive = null, 
        [FromQuery] int? minAvailableSeats = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 10)
    {
        var query = new GetScheduledTourListQuery
        {
            BoatId = boatId,
            Status = status,
            StartDate = startDate,
            EndDate = endDate,
            IsActive = isActive,
            MinAvailableSeats = minAvailableSeats,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.ExecuteQuery<ScheduledTourListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar passeios agendados por ID do barco
    /// </summary>
    [HttpGet("boat/{boatId:guid}")]
    public async Task<IActionResult> GetByBoatIdAsync(
        [FromRoute] Guid boatId, 
        [FromQuery] ScheduledTourStatus? status = null, 
        [FromQuery] DateOnly? startDate = null, 
        [FromQuery] DateOnly? endDate = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 50)
    {
        var query = new GetScheduledTourByBoatIdQuery(boatId)
        {
            Page = page,
            PageSize = pageSize
        };
        
        // Note: GetScheduledTourByBoatIdQuery doesn't support status/date filters
        // These filters would need to be added to the query class

        var result = await _mediator.ExecuteQuery<ScheduledTourListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar passeios agendados disponíveis para reserva
    /// </summary>
    [HttpGet("available")]
    public async Task<IActionResult> GetAvailableAsync(
        [FromQuery] DateOnly? date = null, 
        [FromQuery] Guid? boatId = null, 
        [FromQuery] int page = 1, 
        [FromQuery] int pageSize = 20)
    {
        var query = new GetScheduledTourListQuery
        {
            BoatId = boatId,
            StartDate = date,
            EndDate = date?.AddDays(1),
            IsActive = true,
            MinAvailableSeats = 1,
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.ExecuteQuery<ScheduledTourListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }
}