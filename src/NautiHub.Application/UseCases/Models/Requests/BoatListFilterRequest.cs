namespace NautiHub.Application.UseCases.Models.Requests;

public class BoatListFilterRequest
{
    public string? Search { get; set; }

    public DateTime? CreatedAtStart { get; set; }

    public DateTime? CreatedAtEnd { get; set; }

    public DateTime? UpdatedAtStart { get; set; }

    public DateTime? UpdatedAtEnd { get; set; }

    public int Page { get; set; }

    public int PerPage { get; set; }

    public string? OrderBy { get; set; }
}