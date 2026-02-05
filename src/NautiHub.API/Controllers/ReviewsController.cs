using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Application.UseCases.Features.ReviewCreate;
using NautiHub.Application.UseCases.Features.ReviewUpdate;
using NautiHub.Application.UseCases.Features.ReviewDelete;
using NautiHub.Application.UseCases.Queries.ReviewById;
using NautiHub.Application.UseCases.Queries.ReviewList;
using NautiHub.Application.UseCases.Queries.ReviewByBookingId;
using NautiHub.Application.UseCases.Queries.ReviewByBoatId;
using NautiHub.Core.Controllers;
using NautiHub.Core.Mediator;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para gerenciamento de avaliações
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ReviewsController(IMediatorHandler mediator, MessagesService messagesService) : MainController(mediator, messagesService)
{
    private readonly IMediatorHandler _mediator = mediator;

    /// <summary>
    /// Criar nova avaliação
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateReviewRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new CreateReviewFeature
        {
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Atualizar avaliação
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateReviewRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new UpdateReviewFeature
        {
            ReviewId = id,
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Excluir avaliação
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _mediator.ExecuteFeature(new DeleteReviewFeature
        {
            ReviewId = id
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar avaliação por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var query = new GetReviewByIdQuery(id);
        var result = await _mediator.ExecuteQuery<ReviewResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        if (result.Response == null)
            return NotFound(_messagesService?.Error_Record_Not_Found ?? "Registro não encontrado.");

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Listar avaliações
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] Guid? bookingId = null, [FromQuery] Guid? boatId = null, [FromQuery] Guid? customerId = null, [FromQuery] int? minRating = null, [FromQuery] int? maxRating = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] DateTime? startDate = null, [FromQuery] DateTime? endDate = null)
    {
        var query = new GetReviewListQuery
        {
            BookingId = bookingId,
            BoatId = boatId,
            CustomerId = customerId,
            MinRating = minRating,
            MaxRating = maxRating,
            Page = page,
            PageSize = pageSize,
            StartDate = startDate,
            EndDate = endDate
        };

        var result = await _mediator.ExecuteQuery<ReviewListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar avaliações por ID da reserva
    /// </summary>
    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetByBookingIdAsync([FromRoute] Guid bookingId)
    {
        var query = new GetReviewByBookingIdQuery(bookingId);
        var result = await _mediator.ExecuteQuery<ReviewListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar avaliações por ID do barco
    /// </summary>
    [HttpGet("boat/{boatId:guid}")]
    public async Task<IActionResult> GetByBoatIdAsync([FromRoute] Guid boatId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = new GetReviewByBoatIdQuery(boatId)
        {
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.ExecuteQuery<ReviewListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }
}