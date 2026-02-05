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

namespace NautiHub.Application.UseCases.Features.ReviewUpdate;

/// <summary>
/// Handler para atualização de avaliação
/// </summary>
public class UpdateReviewFeatureHandler : FeatureHandler, IRequestHandler<UpdateReviewFeature, FeatureResponse<ReviewResponse>>
{
    private readonly DatabaseContext _context;
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<UpdateReviewFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public UpdateReviewFeatureHandler(
        DatabaseContext context,
        IReviewRepository reviewRepository,
        ILogger<UpdateReviewFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<ReviewResponse>> Handle(UpdateReviewFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar avaliação
            var review = await _context.Set<Review>().FindAsync(request.ReviewId);
            if (review == null)
            {
                _logger.LogWarning("Avaliação {ReviewId} não encontrada", request.ReviewId);
                AddError(_messagesService.Review_Not_Found);
                return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Atualizar campos permitidos
            review.UpdateRating(request.Data.Rating);
            review.UpdateComment(request.Data.Comment);

            // Salvar no banco
            await _reviewRepository.UpdateAsync(review);
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

            _logger.LogInformation("Avaliação {ReviewId} atualizada com sucesso", review.Id);

            return new FeatureResponse<ReviewResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao atualizar avaliação {ReviewId}", request.ReviewId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<ReviewResponse>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}