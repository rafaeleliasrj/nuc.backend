using NautiHub.Application.UseCases.Events.PaymentHook;
using NautiHub.Core.MessageEvents;

namespace NautiHub.Commin.Configurations;

public static class EventsConfiguration
{
    public static IServiceCollection AddEvents(this IServiceCollection services)
    {
        services.AddScoped<IEventConsumer, EventConsumer>();
        services.AddScoped<IEventPublish, EventPublish>();

        return services;
    }

    public static IApplicationBuilder EventMonitoring(
        this IApplicationBuilder app,
        IEventConsumer eventConsumer,
        ILogger<ProcessWebhookEventHandler> logger,
        IServiceScopeFactory serviceScopeFactory
    )
    {
        eventConsumer.AddConsumer(new ProcessWebhookEventHandler(serviceScopeFactory, logger), simultaneousExecutions: 10).ConfigureAwait(false);

        return app;
    }
}
