using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Features.AtualizarEmpresa;
using NautiHub.Application.UseCases.Features.CadastrarEmpresa;
using NautiHub.Application.UseCases.Features.ExcluirEmpresa;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Application.UseCases.Queries.BoatGetById;
using NautiHub.Application.UseCases.Queries.BoatList;
using NautiHub.Core.Controllers;
using NautiHub.Core.Mediator;
using NautiHub.Core.Messages.Models;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class BoatController(IMediatorHandler mediator, MessagesService messagesService) : MainController(mediator, messagesService)
{
    private readonly IMediatorHandler _mediator = mediator;

    [HttpPost()]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PostAsync([FromBody] BoatRequest body)
    {
        Core.Messages.Features.FeatureResponse<Guid> result = await _mediator.ExecuteFeature(
            new BoatCreateFeature()
            {
                Data = body
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PutAsync(
        [FromRoute] Guid id,
        [FromBody] BoatUpdateRequest body
    )
    {
        var result = await _mediator.ExecuteFeature(
            new BoatUpdateFeature()
            {
                BoatId = id,
                Data = body
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

if (!result.Response)
            return NotFound(_messagesService?.Error_Record_Not_Found ?? "Registro não encontrado.");

        return CustomResponse();
    }

    [HttpPatch("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> PatchAsync(
        [FromRoute] Guid id,
        [FromBody] BoatUpdateRequest body
    )
    {
        var result = await _mediator.ExecuteFeature(
            new BoatUpdateFeature()
            {
                BoatId = id,
                Data = body
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(BoatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetByIdAsync(
        [FromRoute] Guid id
    )
    {
        var result = await _mediator.ExecuteQuery(
            new BoatGetByIdQuery()
            {
                BoatId = id,
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

if (result.Response == null)
            return NotFound(_messagesService?.Error_Record_Not_Found ?? "Registro não encontrado.");

        return CustomResponse(result.Response);
    }

    [HttpGet()]
    [ProducesResponseType(
        typeof(ListPaginationResponse<BoatListResponse>),
        StatusCodes.Status200OK
    )]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ListAsync(
        [FromQuery] string? search,
        [FromQuery] DateTime? createdAtStart,
        [FromQuery] DateTime? createdAtEnd,
        [FromQuery] DateTime? updatedAtStart,
        [FromQuery] DateTime? updatedAtEnd,
        [FromQuery] int page = 1,
        [FromQuery] int perPage = 10,
        [FromQuery] string? orderBy = "CreatedAt.DESC"
    )
    {
        var result = await _mediator.ExecuteQuery(
            new BoatListQuery()
            {
                Filter = new BoatListFilterRequest()
                {
                    Search = search,
                    CreatedAtStart = createdAtStart,
                    CreatedAtEnd = createdAtEnd,
                    UpdatedAtStart = updatedAtStart,
                    UpdatedAtEnd = updatedAtEnd,
                    Page = page,
                    PerPage = perPage,
                    OrderBy = orderBy
                }
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(typeof(BoatResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> DeleteAsync(
        [FromRoute] Guid boatId
    )
    {
        Core.Messages.Features.FeatureResponse<bool> result = await _mediator.ExecuteFeature(
            new BoatDeleteFeature()
            {
                BoatId = boatId,
            }
        );

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }
}
