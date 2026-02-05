namespace NautiHub.Core.Messages.Models;

/// <summary>
/// Informações de paginação
/// </summary>
public class PaginationInfo
{
    /// <summary>
    /// Página atual
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// Tamanho da página
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// Total de páginas
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// Total de registros
    /// </summary>
    public long TotalCount { get; set; }
}

public class ListPaginationResponse<TResponse>
{
    public int CurrentPage { get; set; }

    public int PageCount { get; set; }

    public int PageSize { get; set; }

    public int RowCount { get; set; }

    public List<TResponse> Data { get; set; } = [];

    public long FirstRowOnPage => (CurrentPage - 1) * PageSize + 1;

    public long LastRowOnPage => Math.Min(CurrentPage * PageSize, RowCount);
}

public class ListTotalPaginationResponse<TResponse> : ListPaginationResponse<TResponse>
{
    public decimal Totalization { get; set; }
}