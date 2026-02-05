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

namespace NautiHub.Application.UseCases.Queries.ChatMessageList;

/// <summary>
/// Handler para listar mensagens de chat
/// </summary>
public class GetChatMessageListQueryHandler(
    DatabaseContext context,
    IChatMessageRepository chatMessageRepository,
    ILogger<GetChatMessageListQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetChatMessageListQuery, QueryResponse<ChatMessageListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IChatMessageRepository _chatMessageRepository = chatMessageRepository;
    private readonly ILogger<GetChatMessageListQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ChatMessageListResponse>> Handle(GetChatMessageListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Listar mensagens com paginação e filtros
            var (items, total) = await _chatMessageRepository.ListAsync(
                page: request.Page,
                perPage: request.PageSize,
                search: request.Search,
                bookingId: request.BookingId,
                senderId: request.SenderId,
                recipientId: null,
                createdAtStart: request.CreatedAtStart,
                createdAtEnd: request.CreatedAtEnd,
                orderBy: request.OrderBy);

            // Mapear para response
            var messages = items.Select(chatMessage => new ChatMessageResponse
            {
                Id = chatMessage.Id,
                BookingId = chatMessage.BookingId,
                SenderId = chatMessage.SenderId,
                Message = chatMessage.Message,
                IsRead = chatMessage.IsRead,
                CreatedAt = chatMessage.CreatedAt!.Value,
                UpdatedAt = chatMessage.UpdatedAt
            }).ToList();

            var response = new ChatMessageListResponse
            {
                Messages = messages,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize)
            };

            return new QueryResponse<ChatMessageListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar mensagens de chat");
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("List", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ChatMessageListResponse>(validationResult);
        }
    }
}