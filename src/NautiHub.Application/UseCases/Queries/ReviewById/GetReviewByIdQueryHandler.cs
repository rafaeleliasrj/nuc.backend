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

namespace NautiHub.Application.UseCases.Queries.ReviewById;

/// <summary>
/// Handler para buscar avaliação por ID
/// </summary>
public class GetReviewByIdQueryHandler(
    DatabaseContext context,
    IReviewRepository reviewRepository,
    ILogger<GetReviewByIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetReviewByIdQuery, QueryResponse<ReviewResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly ILogger<GetReviewByIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ReviewResponse>> Handle(GetReviewByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar avaliação
            var review = await _context.Set<Review>().FindAsync(request.Id);
            if (review == null)
            {
                var validationResult = new ValidationResult();
                validationResult.Errors.Add(new ValidationFailure("ReviewId", _messagesService.Review_Not_Found));
                return new QueryResponse<ReviewResponse>(validationResult);
            }

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

            return new QueryResponse<ReviewResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar avaliação {ReviewId}", request.Id);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("ReviewId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ReviewResponse>(validationResult);
        }
    }
}