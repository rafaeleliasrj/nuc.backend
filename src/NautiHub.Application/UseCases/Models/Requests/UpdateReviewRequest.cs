namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Request para atualização de avaliação
/// </summary>
public class UpdateReviewRequest
{
    /// <summary>
    /// Classificação atualizada da avaliação (1 a 5 estrelas)
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// Comentário atualizado da avaliação
    /// </summary>
    public string? Comment { get; set; }
}