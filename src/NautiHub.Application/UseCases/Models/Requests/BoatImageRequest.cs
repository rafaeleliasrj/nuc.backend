namespace NautiHub.Application.UseCases.Models.Requests;

/// <summary>
/// Atualizar logo da empresa.
/// </summary>
public class BoatImageRequest
{
    /// <summary>
    /// Logo em Base 64 da empresa.
    /// </summary>
    public string Logo { get; set; }
}