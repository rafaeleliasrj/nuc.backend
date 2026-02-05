using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Query;

using System.Linq.Expressions;
using System.Reflection;

namespace NautiHub.Infrastructure.Services.Utilitarios;

public static class ModelBuilderExtensions
{
    private static readonly MethodInfo SetQueryFilterMethod = typeof(ModelBuilderExtensions)
        .GetMethods(BindingFlags.NonPublic | BindingFlags.Static)
        .Single(m => m.IsGenericMethod && m.Name == nameof(SetQueryFilter));

    public static void SetQueryFilterOnAllEntities<TEntityInterface>(
        this ModelBuilder builder,
        Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        IEnumerable<Type> entityTypes = builder.Model.GetEntityTypes()
            .Where(t => t.BaseType == null)
            .Select(t => t.ClrType)
            .Where(t => typeof(TEntityInterface).IsAssignableFrom(t));

        foreach (Type? type in entityTypes)
            builder.SetEntityQueryFilter(type, filterExpression);
    }

    private static void SetEntityQueryFilter<TEntityInterface>(
        this ModelBuilder builder,
        Type entityType,
        Expression<Func<TEntityInterface, bool>> filterExpression)
    {
        MethodInfo genericMethod = SetQueryFilterMethod.MakeGenericMethod(entityType, typeof(TEntityInterface));
        genericMethod.Invoke(null, new object[] { builder, filterExpression });
    }

    private static void SetQueryFilter<TEntity, TEntityInterface>(
        this ModelBuilder builder,
        Expression<Func<TEntityInterface, bool>> filterExpression)
        where TEntity : class, TEntityInterface
        where TEntityInterface : class
    {
        Expression<Func<TEntity, bool>> convertedExpression = filterExpression.Convert<TEntityInterface, TEntity>();
        builder.Entity<TEntity>().AppendQueryFilter(convertedExpression);
    }

    private static void AppendQueryFilter<TEntity>(
        this EntityTypeBuilder<TEntity> entityTypeBuilder,
        Expression<Func<TEntity, bool>> newFilter)
        where TEntity : class
    {
        LambdaExpression? currentFilter = entityTypeBuilder.Metadata.GetQueryFilter();
        if (currentFilter != null)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity));

            Expression replacedCurrent = ReplacingExpressionVisitor.Replace(
                currentFilter.Parameters.Single(), parameter, currentFilter.Body);

            Expression replacedNew = ReplacingExpressionVisitor.Replace(
                newFilter.Parameters.Single(), parameter, newFilter.Body);

            BinaryExpression combined = Expression.AndAlso(replacedCurrent, replacedNew);
            var lambda = Expression.Lambda<Func<TEntity, bool>>(combined, parameter);

            entityTypeBuilder.HasQueryFilter(lambda);
        }
        else
        {
            entityTypeBuilder.HasQueryFilter(newFilter);
        }
    }

    public static Expression<Func<TTarget, bool>> Convert<TSource, TTarget>(
        this Expression<Func<TSource, bool>> source)
        where TTarget : TSource
    {
        ParameterExpression parameter = Expression.Parameter(typeof(TTarget), source.Parameters[0].Name);
        Expression body = ReplacingExpressionVisitor.Replace(source.Parameters[0], parameter, source.Body);
        return Expression.Lambda<Func<TTarget, bool>>(body, parameter);
    }
}
