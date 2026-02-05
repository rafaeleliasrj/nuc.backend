using System.Linq.Expressions;
using System.Reflection;

namespace NautiHub.Core.Extensions;

public static class OrdenacaoExtensions
{
    public static IQueryable<T> ApplyOrder<T>(
        this IQueryable<T> query,
        string orderBy,
        Dictionary<string, string>? aliasOrder = null
    )
    {
        if (string.IsNullOrWhiteSpace(orderBy))
            return query;

        var criteria = orderBy.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var firstOrder = true;

        foreach (var criterion in criteria)
        {
            var parts = criterion.Trim().Split('.');
            if (parts.Length < 1)
                continue;

            var direction = parts[^1].ToUpper() is "DESC" or "ASC" ? parts[^1].ToUpper() : "ASC";
            var prop =
                direction == "ASC" || direction == "DESC"
                    ? string.Join('.', parts.Take(parts.Length - 1))
                    : string.Join('.', parts);

            if (aliasOrder != null && aliasOrder.TryGetValue(prop, out var mapeado))
                prop = mapeado;

            Expression<Func<T, object>>? expression = CreateOrderExpression<T>(prop);
            if (expression == null)
                continue;

            if (firstOrder)
            {
                query =
                    direction == "DESC"
                        ? Queryable.OrderByDescending(query, expression)
                        : Queryable.OrderBy(query, expression);
                firstOrder = false;
            }
            else
            {
                query =
                    direction == "DESC"
                        ? Queryable.ThenByDescending((IOrderedQueryable<T>)query, expression)
                        : Queryable.ThenBy((IOrderedQueryable<T>)query, expression);
            }
        }

        return query;
    }

    private static Expression<Func<T, object>>? CreateOrderExpression<T>(string way)
    {
        ParameterExpression param = Expression.Parameter(typeof(T), "x");
        Expression body = param;

        foreach (var prop in way.Split('.'))
        {
            PropertyInfo? propInfo = body.Type.GetProperty(
                prop,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase
            );
            if (propInfo == null)
                return null;

            body = Expression.Property(body, propInfo);
        }

        if (body.Type.IsValueType)
            body = Expression.Convert(body, typeof(object));

        return Expression.Lambda<Func<T, object>>(body, param);
    }
}
