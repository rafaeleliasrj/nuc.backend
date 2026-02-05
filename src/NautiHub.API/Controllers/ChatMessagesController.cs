using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Application.UseCases.Features.ChatMessageCreate;
using NautiHub.Application.UseCases.Features.ChatMessageUpdate;
using NautiHub.Application.UseCases.Features.ChatMessageDelete;
using NautiHub.Application.UseCases.Queries.ChatMessageById;
using NautiHub.Application.UseCases.Queries.ChatMessageList;
using NautiHub.Application.UseCases.Queries.ChatMessageByBookingId;
using NautiHub.Core.Controllers;
using NautiHub.Core.Mediator;
using NautiHub.Core.Resources;

namespace NautiHub.API.Controllers;

/// <summary>
/// Controller para gerenciamento de mensagens de chat
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[Authorize]
public class ChatMessagesController(IMediatorHandler mediator, MessagesService messagesService) : MainController(mediator, messagesService)
{
    private readonly IMediatorHandler _mediator = mediator;

    /// <summary>
    /// Criar nova mensagem de chat
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateChatMessageRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new CreateChatMessageFeature
        {
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Atualizar mensagem de chat
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] UpdateChatMessageRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new UpdateChatMessageFeature
        {
            MessageId = id,
            Data = body
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Marcar mensagem como lida/não lida
    /// </summary>
    [HttpPatch("{id:guid}/read")]
    public async Task<IActionResult> MarkAsReadAsync([FromRoute] Guid id, [FromBody] MarkChatMessageReadRequest body)
    {
        if (!ModelState.IsValid)
            return CustomResponse(ModelState);

        var result = await _mediator.ExecuteFeature(new UpdateChatMessageFeature
        {
            MessageId = id,
            Data = new UpdateChatMessageRequest 
            { 
                Message = body.IsRead ? _messagesService?.ChatMessage_Marked_Read ?? "Mensagem marcada como lida" : _messagesService?.ChatMessage_Marked_Unread ?? "Mensagem marcada como não lida"
            }
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Excluir mensagem de chat
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        var result = await _mediator.ExecuteFeature(new DeleteChatMessageFeature
        {
            MessageId = id
        });

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult, result.StatusCode);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar mensagem por ID
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        var query = new GetChatMessageByIdQuery(id);
        var result = await _mediator.ExecuteQuery<ChatMessageResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        if (result.Response == null)
            return NotFound(_messagesService?.Error_Record_Not_Found ?? "Registro não encontrado.");

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Listar mensagens de chat
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> ListAsync([FromQuery] Guid? bookingId = null, [FromQuery] Guid? senderId = null, [FromQuery] string? search = null, [FromQuery] int page = 1, [FromQuery] int pageSize = 10, [FromQuery] DateTime? createdAtStart = null, [FromQuery] DateTime? createdAtEnd = null, [FromQuery] string? orderBy = null)
    {
        var query = new GetChatMessageListQuery
        {
            BookingId = bookingId,
            SenderId = senderId,
            Search = search,
            Page = page,
            PageSize = pageSize,
            CreatedAtStart = createdAtStart,
            CreatedAtEnd = createdAtEnd,
            OrderBy = orderBy
        };

        var result = await _mediator.ExecuteQuery<ChatMessageListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }

    /// <summary>
    /// Buscar mensagens por ID da reserva
    /// </summary>
    [HttpGet("booking/{bookingId:guid}")]
    public async Task<IActionResult> GetByBookingIdAsync([FromRoute] Guid bookingId, [FromQuery] int page = 1, [FromQuery] int pageSize = 50)
    {
        var query = new GetChatMessageByBookingIdQuery(bookingId)
        {
            Page = page,
            PageSize = pageSize
        };

        var result = await _mediator.ExecuteQuery<ChatMessageListResponse>(query);

        if (!result.ValidationResult.IsValid)
            return CustomResponse(result.ValidationResult);

        return CustomResponse(result.Response);
    }
}