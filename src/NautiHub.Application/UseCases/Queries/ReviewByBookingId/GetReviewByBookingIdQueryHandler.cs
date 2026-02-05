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

namespace NautiHub.Application.UseCases.Queries.ReviewByBookingId;

/// <summary>
/// Handler para buscar avaliações por ID da reserva
/// </summary>
public class GetReviewByBookingIdQueryHandler(
    DatabaseContext context,
    IReviewRepository reviewRepository,
    ILogger<GetReviewByBookingIdQueryHandler> logger,
    MessagesService messagesService) : QueryHandler, IRequestHandler<GetReviewByBookingIdQuery, QueryResponse<ReviewListResponse>>
{
    private readonly DatabaseContext _context = context;
    private readonly IReviewRepository _reviewRepository = reviewRepository;
    private readonly ILogger<GetReviewByBookingIdQueryHandler> _logger = logger;
    private readonly MessagesService _messagesService = messagesService;

    public async Task<QueryResponse<ReviewListResponse>> Handle(GetReviewByBookingIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar avaliações por ID da reserva
            var items = await _reviewRepository.GetByBookingIdAsync(request.BookingId);
            
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
                    BookingId = request.BookingId
                }
            };

            return new QueryResponse<ReviewListResponse>(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar avaliações para reserva {BookingId}", request.BookingId);
            var validationResult = new ValidationResult();
            validationResult.Errors.Add(new ValidationFailure("BookingId", _messagesService.Error_Internal_Server_Generic));
            return new QueryResponse<ReviewListResponse>(validationResult);
        }
    }
}