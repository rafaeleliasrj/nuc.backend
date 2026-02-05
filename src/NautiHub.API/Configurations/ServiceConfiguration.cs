namespace NautiHub.API.Configurations;

internal static class ServicesConfiguration
{
    public static void AddServicesConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCommonServicesConfiguration(configuration);
    }
}
