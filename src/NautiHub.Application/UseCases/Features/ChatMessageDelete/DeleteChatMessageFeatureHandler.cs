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

namespace NautiHub.Application.UseCases.Features.ChatMessageDelete;

/// <summary>
/// Handler para exclusão de mensagem de chat
/// </summary>
public class DeleteChatMessageFeatureHandler : FeatureHandler, IRequestHandler<DeleteChatMessageFeature, FeatureResponse<bool>>
{
    private readonly DatabaseContext _context;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly ILogger<DeleteChatMessageFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public DeleteChatMessageFeatureHandler(
        DatabaseContext context,
        IChatMessageRepository chatMessageRepository,
        ILogger<DeleteChatMessageFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _chatMessageRepository = chatMessageRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<bool>> Handle(DeleteChatMessageFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar mensagem de chat
            var chatMessage = await _context.Set<ChatMessage>().FindAsync(request.MessageId);
            if (chatMessage == null)
            {
                _logger.LogWarning("Mensagem de chat {MessageId} não encontrada", request.MessageId);
                AddError(_messagesService.ChatMessage_Not_Found);
                return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Marcar como deletada (soft delete)
            chatMessage.MarkAsDeleted();

            // Salvar no banco
            await _chatMessageRepository.UpdateAsync(chatMessage);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Mensagem de chat {MessageId} excluída com sucesso", chatMessage.Id);

            return new FeatureResponse<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir mensagem de chat {MessageId}", request.MessageId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}