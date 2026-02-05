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

namespace NautiHub.Application.UseCases.Queries.ChatMessageByBookingId;

/// <summary>
/// Handler para buscar mensagens de chat por ID da reserva
/// </summary>
public class GetChatMessageByBookingIdQueryHandler(
    DatabaseContext context,
    IChatMessageRepository chatMessageRepository,
    ILogger<GetChatMessageByBookingIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetChatMessageByBookingIdQuery, QueryResponse<ChatMessageListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IChatMessageRepository _chatMessageRepository = chatMessageRepository;
    private readonly ILogger<GetChatMessageByBookingIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ChatMessageListResponse>> Handle(GetChatMessageByBookingIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar mensagens por ID da reserva
            var items = await _chatMessageRepository.GetByBookingIdAsync(request.BookingId);
            
            // Aplicar paginação
            var total = items.Count();
            var pagedItems = items
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Mapear para response
            var messages = pagedItems.Select(chatMessage => new ChatMessageResponse
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
            _logger.LogError(ex, "Erro ao buscar mensagens de chat para reserva {BookingId}", request.BookingId);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("BookingId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ChatMessageListResponse>(validationResult);
        }
    }
}