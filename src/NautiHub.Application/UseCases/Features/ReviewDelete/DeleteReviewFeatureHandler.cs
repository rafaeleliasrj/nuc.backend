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

namespace NautiHub.Application.UseCases.Features.ReviewDelete;

/// <summary>
/// Handler para exclusão de avaliação
/// </summary>
public class DeleteReviewFeatureHandler : FeatureHandler, IRequestHandler<DeleteReviewFeature, FeatureResponse<bool>>
{
    private readonly DatabaseContext _context;
    private readonly IReviewRepository _reviewRepository;
    private readonly ILogger<DeleteReviewFeatureHandler> _logger;
    private readonly MessagesService _messagesService;

    public DeleteReviewFeatureHandler(
        DatabaseContext context,
        IReviewRepository reviewRepository,
        ILogger<DeleteReviewFeatureHandler> logger,
        MessagesService messagesService) : base(context)
    {
        _context = context;
        _reviewRepository = reviewRepository;
        _logger = logger;
        _messagesService = messagesService;
    }

    public async Task<FeatureResponse<bool>> Handle(DeleteReviewFeature request, CancellationToken cancellationToken)
    {
        try
        {
            // Buscar avaliação
            var review = await _context.Set<Review>().FindAsync(request.ReviewId);
            if (review == null)
            {
                _logger.LogWarning("Avaliação {ReviewId} não encontrada", request.ReviewId);
                AddError(_messagesService.Review_Not_Found);
                return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.NotFound);
            }

            // Marcar como deletada (soft delete)
            review.MarkAsDeleted();

            // Salvar no banco
            await _reviewRepository.UpdateAsync(review);
            await _context.SaveChangesAsync();

            _logger.LogInformation("Avaliação {ReviewId} excluída com sucesso", review.Id);

            return new FeatureResponse<bool>(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao excluir avaliação {ReviewId}", request.ReviewId);
            AddError(_messagesService.Error_Internal_Server_Generic);
            return new FeatureResponse<bool>(ValidationResult, statusCode: HttpStatusCode.InternalServerError);
        }
    }
}