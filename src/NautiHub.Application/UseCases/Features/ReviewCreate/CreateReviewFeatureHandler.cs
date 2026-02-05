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
using Microsoft.EntityFrameworkCore;

namespace NautiHub.Application.UseCases.Features.ReviewCreate;

/// <summary>
/// Handler para criação de avaliação
/// </summary>
public class CreateReviewFeatureHandler : FeatureHandler, IRequestHandler<CreateReviewFeature, FeatureResponse<ReviewResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IReviewRepository _reviewRepository;
    private readonly IBookingRepository _bookingRepository;
    private readonly IBoatRepository _boatRepository;
    private readonly ILogger<CreateReviewFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public CreateReviewFeatureHandler(
        DatabaseContext context,
        IReviewRepository reviewRepository,
        IBookingRepository bookingRepository,
        IBoatRepository boatRepository,
        ILogger<CreateReviewFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _bookingRepository = bookingRepository;
        _boatRepository = boatRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ReviewResponse>> Handle(CreateReviewFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Validar se a reserva existe
            var booking = await _context.Set<Booking>().FindAsync(request.Data.BookingId);
            if (booking == null)
            {
                _logger.LogWarning("Reserva {BookingId} não encontrada", request.Data.BookingId);
                AddError(_messagesService.Payment_Booking_Not_Found);
                return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Validar se o barco existe
            var boat = await _context.Set<Boat>().FindAsync(request.Data.BoatId);
            if (boat == null)
            {
                _logger.LogWarning("Barco {BoatId} não encontrado", request.Data.BoatId);
                AddError(_messagesService.Boat_Not_Found);
                return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Verificar se já existe avaliação para esta reserva
            var existingReview = await _context.Set<Review>()
                .FirstOrDefaultAsync(r => r.BookingId == request.Data.BookingId);
            
            if (existingReview != null)
            {
                _logger.LogWarning("Já existe avaliação para a reserva {BookingId}", request.Data.BookingId);
                AddError(_messagesService.Review_Already_Exists);
                return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.BadRequest);
            }

            // Criar avaliação
            var review = new Review(
                request.Data.BookingId,
                request.Data.BoatId,
                request.Data.CustomerId,
                request.Data.Rating,
                request.Data.Comment);

            // Salvar no banco
            await _reviewRepository.AddAsync(review);
            await _context.SaveChangesAsync();

            // Mapear para response
            var response = new ReviewResponse
            {
                Id = review.Id,
                BookingId = review.BookingId,
                BoatId = review.BoatId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt!.Value,
                UpdatedAt = review.UpdatedAt
            };

            _logger.LogInformation("Avaliação {ReviewId} criada com sucesso para reserva {BookingId}", 
                review.Id, review.BookingId);

            return new FeatureResponse<ReviewResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao criar avaliação para reserva {BookingId}", request.Data.BookingId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}