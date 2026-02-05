using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Refit;
using NautiHub.Infrastructure.Gateways.Asaas;
using NautiHub.Infrastructure.Gateways.Asaas.DTOs;
using System.Net.Http;

namespace NautiHub.Infrastructure.Gateways.Asaas.Configuration;

/// <summary>
/// Configuração dos serviços do Asaas
/// </summary>
public static class AsaasServicesConfiguration
{
    /// <summary>
    /// Adicionar serviços do Asaas ao container de DI
    /// </summary>
    public static IServiceCollection AddAsaasServices(
        this IServiceCollection services, 
        AsaasSettings settings)
    {
        // Registrar configurações
        services.Configure<AsaasSettings>(options =>
        {
            options.BaseUrl = settings.BaseUrl;
            options.ApiKey = settings.ApiKey;
            options.Environment = settings.Environment;
            options.WebhookUrl = settings.WebhookUrl;
            options.WebhookToken = settings.WebhookToken;
            options.TimeoutInSeconds = settings.TimeoutInSeconds;
        });

        // Configurar HttpClient para Refit (já configurado acima)

        services.AddRefitClient<IAsaasPaymentsApi>()
        .ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);
            client.DefaultRequestHeaders.Add("access_token", settings.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "NautiHub/1.0");
        });

        services.AddRefitClient<IAsaasCustomersApi>()
        .ConfigureHttpClient(client =>
        {
            client.BaseAddress = new Uri(settings.BaseUrl);
            client.Timeout = TimeSpan.FromSeconds(settings.TimeoutInSeconds);
            client.DefaultRequestHeaders.Add("access_token", settings.ApiKey);
            client.DefaultRequestHeaders.Add("User-Agent", "NautiHub/1.0");
        });

        // Registrar serviço principal
        services.AddScoped<IAsaasService, AsaasService>();

        return services;
    }

    /// <summary>
    /// Adicionar serviços do Asaas com configuração do appsettings
    /// </summary>
    public static IServiceCollection AddAsaasServices(
        this IServiceCollection services,
        Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        // Obter configurações do appsettings
        var settings = new AsaasSettings();
        configuration.GetSection("Asaas").Bind(settings);

        return services.AddAsaasServices(settings);
    }
}