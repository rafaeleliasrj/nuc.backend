using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace NautiHub.API.Binders;

/// <inheritdoc/>
public class DelimitedListModelBinderProvider : IModelBinderProvider
{
    /// <inheritdoc/>
    public IModelBinder? GetBinder(ModelBinderProviderContext context)
    {
        if (!context.Metadata.IsEnumerableType)
            return null;

        Type elementType = context.Metadata.ElementType!;
        Type binderType = typeof(DelimitedListModelBinder<>).MakeGenericType(elementType);
        return (IModelBinder)Activator.CreateInstance(binderType)!;
    }
}
