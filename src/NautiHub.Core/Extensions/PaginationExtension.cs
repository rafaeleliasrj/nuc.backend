using Microsoft.EntityFrameworkCore;
using NautiHub.Core.Messages.Models;

namespace NautiHub.Core.Extensions;

public static class PaginationExtension
{
    public static int RetornaNumeroDePaginas(this int totalDeRegistros, int limit)
    {
        var totalDePagina = (int)((decimal)(totalDeRegistros / limit)).Truncate(0);

        var restoDivisaoPaginasIgualZero = totalDeRegistros % limit == 0;

        if (restoDivisaoPaginasIgualZero)
        {
            return totalDePagina;
        }

        totalDePagina++;

        return totalDePagina;
    }

    public static async Task<ListPaginationResponse<TEntity>> GetPaginated<TEntity>(this IQueryable<TEntity> query, int pagina, int registrosPorPagina) where TEntity : class
    {
        var registros = new ListPaginationResponse<TEntity>();
        registros.CurrentPage = pagina;
        registros.PageCount = registrosPorPagina;
        registros.RowCount = query.Count();

        var totalPaginas = (double)registros.RowCount / registrosPorPagina;
        registros.PageSize = (int)Math.Ceiling(totalPaginas);

        var skip = (pagina - 1) * registrosPorPagina;

        registros.Data = await query.Skip(skip).Take(registrosPorPagina).ToListAsync();

        return registros;
    }
}
