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

namespace NautiHub.Application.UseCases.Queries.ChatMessageById;

/// <summary>
/// Handler para buscar mensagem de chat por ID
/// </summary>
public class GetChatMessageByIdQueryHandler : QueryHandler, IRequestHandler<GetChatMessageByIdQuery, QueryResponse<ChatMessageResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly ILogger<GetChatMessageByIdQueryHandler> _logger;
    private readonly MessagesService _messagesService;

    public GetChatMessageByIdQueryHandler(
        DatabaseContext context,
        IChatMessageRepository chatMessageRepository,
        ILogger<GetChatMessageByIdQueryHandler> logger,
        MessagesService messagesService)
    {
        _context = context;
        _chatMessageRepository = chatMessageRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<QueryResponse<ChatMessageResponse>> Handle(GetChatMessageByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar mensagem de chat
            var chatMessage = await _context.Set<ChatMessage>().FindAsync(request.Id);
            if (chatMessage == null)
            {
                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationFailure("MessageId", _messagesService.ChatMessage_Not_Found));
                return new QueryResponse<ChatMessageResponse>(validationResult);
            }

            // Mapear para response
            var response = new ChatMessageResponse
            {
                Id = chatMessage.Id,
                BookingId = chatMessage.BookingId,
                SenderId = chatMessage.SenderId,
                Message = chatMessage.Message,
                IsRead = chatMessage.IsRead,
                CreatedAt = chatMessage.CreatedAt!.Value,
                UpdatedAt = chatMessage.UpdatedAt
            };

            return new QueryResponse<ChatMessageResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar mensagem de chat {MessageId}", request.Id);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("MessageId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ChatMessageResponse>(validationResult);
        }
    }
}