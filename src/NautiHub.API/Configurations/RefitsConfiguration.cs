using NautiHub.Infrastructure.Services.Ibge.Servers.Refit;
using NautiHub.Infrastructure.Services.Utilitarios;
using NautiHub.Infrastructure.Gateways.Asaas;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Refit;

namespace NautiHub.API.Configurations;

public static class RefitsConfiguration
{
    public static void AddRefitsConfiguration(this IServiceCollection services)
    {
        services.AddTransient<LoggingHandler>();

        // ===== SERIALIZADOR PADRÃO =====
        var jsonSettings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore,
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        var refitSettings = new RefitSettings
        {
            ContentSerializer = new NewtonsoftJsonContentSerializer(jsonSettings)
        };

        // ===== IBGE =====
        //TODO: Ajustar quando subir a variável de ambiente
        var ibgeUrl = "https://servicodados.ibge.gov.br"; //GetEnv("IBGE_BASEURI");

        services
            .AddRefitClient<IIbgeRefit>()
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    client.BaseAddress = new Uri(ibgeUrl);
                    client.Timeout = TimeSpan.FromSeconds(10);
                }
            );

        // ===== ASAAS PAYMENT GATEWAY =====
        var asaasBaseUrl = Environment.GetEnvironmentVariable("ASAAS_API_URL") ?? "https://api-sandbox.asaas.com/v3/";

        services
            .AddRefitClient<IAsaasCustomersApi>(refitSettings)
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    client.BaseAddress = new Uri(asaasBaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(30);
                }
            );

        services
            .AddRefitClient<IAsaasPaymentsApi>(refitSettings)
            .ConfigureHttpClient(
                (provider, client) =>
                {
                    client.BaseAddress = new Uri(asaasBaseUrl);
                    client.Timeout = TimeSpan.FromSeconds(30);
                }
            );
    }
}
