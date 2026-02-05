using System.Reflection;

namespace NautiHub.Domain.Services.InfrastructureService.Utilitarios;

public static class ObjectExtensions
{
    public static bool TemAlgumValor<T>(this T obj)
    {
        if (obj == null)
            return false;

        PropertyInfo[] properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return properties.Any(p => p.GetValue(obj) != null);
    }
}
