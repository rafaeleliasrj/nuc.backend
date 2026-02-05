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

namespace NautiHub.Application.UseCases.Features.ChatMessageCreate;

/// <summary>
/// Handler para criação de mensagem de chat
/// </summary>
public class CreateChatMessageFeatureHandler : FeatureHandler, IRequestHandler<CreateChatMessageFeature, FeatureResponse<ChatMessageResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IChatMessageRepository _chatMessageRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly ILogger<CreateChatMessageFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public CreateChatMessageFeatureHandler(
        DatabaseContext context,
        IChatMessageRepository chatMessageRepository,
        IBookingRepository bookingRepository,
        ILogger<CreateChatMessageFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _chatMessageRepository = chatMessageRepository;
        _bookingRepository = bookingRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ChatMessageResponse>> Handle(CreateChatMessageFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Validar se a reserva existe
            var booking = await _context.Set<Booking>().FindAsync(request.Data.BookingId);
            if (booking == null)
            {
                _logger.LogWarning("Reserva {BookingId} não encontrada", request.Data.BookingId);
                AddError(_messagesService.Payment_Booking_Not_Found);
                return new FeatureResponse<ChatMessageResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Criar mensagem de chat
            var chatMessage = new ChatMessage(
                request.Data.BookingId,
                request.Data.SenderId,
                request.Data.Message);

            // Salvar no banco
            await _chatMessageRepository.AddAsync(chatMessage);
            await _context.SaveChangesAsync();

            // Mapear para response
            var response = new ChatMessageResponse
            {
                Id = chatMessage.Id,
                BookingId = chatMessage.BookingId,
                SenderId = chatMessage.SenderId,
                Message = chatMessage.Message,
                IsRead = chatMessage.IsRead,
                CreatedAt = chatMessage.CreatedAt ?? DateTime.Now,
                UpdatedAt = chatMessage.UpdatedAt!.Value
            };

            _logger.LogInformation("Mensagem de chat {MessageId} criada com sucesso para reserva {BookingId}", 
                chatMessage.Id, chatMessage.BookingId);

            return new FeatureResponse<ChatMessageResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar mensagem de chat para reserva {BookingId}", request.Data.BookingId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ChatMessageResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}