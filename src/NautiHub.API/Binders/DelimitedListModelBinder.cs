using Microsoft.AspNetCore.Mvc.ModelBinding;

using System.ComponentModel;
using System.Globalization;

namespace NautiHub.API.Binders;

/// <inheritdoc/>
public class DelimitedListModelBinder<T> : IModelBinder
{
    /// <inheritdoc/>
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        ValueProviderResult valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

        if (valueProviderResult == ValueProviderResult.None)
        {
            bindingContext.Result = ModelBindingResult.Success(new List<T>());
            return Task.CompletedTask;
        }

        Microsoft.Extensions.Primitives.StringValues rawValues = valueProviderResult.Values;

        var allValues = new List<string>();

        foreach (var raw in rawValues)
        {
            if (string.IsNullOrWhiteSpace(raw)) continue;

            if (raw.Contains(','))
                allValues.AddRange(raw.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            else
                allValues.Add(raw.Trim());
        }

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));
        var typedList = new List<T>();

        foreach (var item in allValues)
        {
            var converted = (T)converter.ConvertFromString(null, CultureInfo.InvariantCulture, item)!;
            typedList.Add(converted);
        }

        bindingContext.Result = ModelBindingResult.Success(typedList);
        return Task.CompletedTask;
    }
}
