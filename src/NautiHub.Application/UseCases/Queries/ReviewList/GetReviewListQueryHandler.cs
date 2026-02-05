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

namespace NautiHub.Application.UseCases.Queries.ReviewList;

/// <summary>
/// Handler para listar avaliações
/// </summary>
public class GetReviewListQueryHandler(
    DatabaseContext context,
    IReviewRepository reviewRepository,
    ILogger<GetReviewListQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetReviewListQuery, QueryResponse<ReviewListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly ILogger<GetReviewListQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ReviewListResponse>> Handle(GetReviewListQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Listar avaliações com paginação e filtros
            var (items, total) = await _reviewRepository.ListAsync(
                page: request.Page,
                perPage: request.PageSize,
                search: null,
                boatId: request.BoatId,
                reviewerId: request.CustomerId,
                revieweeId: null,
                minRating: request.MinRating,
                maxRating: request.MaxRating,
                createdAtStart: request.StartDate,
                createdAtEnd: request.EndDate,
                orderBy: null);

            // Calcular média de avaliações
            double averageRating = 0;
            if (items.Any())
                averageRating = items.Average(r => r.Rating);

            // Mapear para response
            var reviews = items.Select(review => new ReviewResponse
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
                    BookingId = request.BookingId,
                    BoatId = request.BoatId,
                    CustomerId = request.CustomerId,
                    MinRating = request.MinRating,
                    MaxRating = request.MaxRating,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate
                }
            };

            return new QueryResponse<ReviewListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao listar avaliações");
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("List", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ReviewListResponse>(validationResult);
        }
    }
}