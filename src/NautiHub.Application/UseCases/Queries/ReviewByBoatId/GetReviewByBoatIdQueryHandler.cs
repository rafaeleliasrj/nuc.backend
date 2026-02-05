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

namespace NautiHub.Application.UseCases.Queries.ReviewByBoatId;

/// <summary>
/// Handler para buscar avaliações por ID do barco
/// </summary>
public class GetReviewByBoatIdQueryHandler(
    DatabaseContext context,
    IReviewRepository reviewRepository,
    ILogger<GetReviewByBoatIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetReviewByBoatIdQuery, QueryResponse<ReviewListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly ILogger<GetReviewByBoatIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ReviewListResponse>> Handle(GetReviewByBoatIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar avaliações por ID do barco
            var items = await _reviewRepository.GetByBoatIdAsync(request.BoatId);
            
            // Aplicar paginação
            var total = items.Count();
            var pagedItems = items
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToList();

            // Calcular média de avaliações
            double averageRating = 0;
            if (items.Any())
                averageRating = items.Average(r => r.Rating);

            // Mapear para response
            var reviews = pagedItems.Select(review => new ReviewResponse
            {
                Id = review.Id,
                BookingId = review.BookingId,
                BoatId = review.BoatId,
                CustomerId = review.CustomerId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt!.Value,
                UpdatedAt = review.UpdatedAt
            }).ToList();

            var response = new ReviewListResponse
            {
                Reviews = reviews,
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                AverageRating = averageRating,
                Filters = new ReviewFilters
                {
                    BoatId = request.BoatId
                }
            };

            return new QueryResponse<ReviewListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar avaliações para barco {BoatId}", request.BoatId);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("BoatId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ReviewListResponse>(validationResult);
        }
    }
}