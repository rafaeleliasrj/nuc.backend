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

namespace NautiHub.Application.UseCases.Features.ChatMessageUpdate;

/// <summary>
/// Handler para atualização de mensagem de chat
/// </summary>
public class UpdateChatMessageFeatureHandler : FeatureHandler, IRequestHandler<UpdateChatMessageFeature, FeatureResponse<ChatMessageResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly ILogger<UpdateChatMessageFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public UpdateChatMessageFeatureHandler(
        DatabaseContext context,
        IChatMessageRepository chatMessageRepository,
        ILogger<UpdateChatMessageFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _chatMessageRepository = chatMessageRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ChatMessageResponse>> Handle(UpdateChatMessageFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar mensagem de chat
            var chatMessage = await _context.Set<ChatMessage>().FindAsync(request.MessageId);
            if (chatMessage == null)
            {
                _logger.LogWarning("Mensagem de chat {MessageId} não encontrada", request.MessageId);
                AddError(_messagesService.ChatMessage_Not_Found);
                return new FeatureResponse<ChatMessageResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Atualizar mensagem
            chatMessage.UpdateMessage(request.Data.Message);

            // Salvar no banco
            await _chatMessageRepository.UpdateAsync(chatMessage);
            await _context.SaveChangesAsync();

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

            _logger.LogInformation("Mensagem de chat {MessageId} atualizada com sucesso", chatMessage.Id);

            return new FeatureResponse<ChatMessageResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar mensagem de chat {MessageId}", request.MessageId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ChatMessageResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}