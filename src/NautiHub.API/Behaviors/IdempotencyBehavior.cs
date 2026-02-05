using MediatR;
using Microsoft.Extensions.Logging;
using NautiHub.Core.Idempotency;
using NautiHub.CrossCutting.Services.Idempotency;

namespace NautiHub.Core.Behaviors;

public class IdempotencyBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
where TRequest : IRequest<TResponse>
{
    private readonly IIdempotencyService _idempotencyService;
    private readonly ILogger<IdempotencyBehavior<TRequest, TResponse>> _logger;

    public IdempotencyBehavior(
        IIdempotencyService idempotencyService,
        ILogger<IdempotencyBehavior<TRequest, TResponse>> logger)
    {
        _idempotencyService = idempotencyService;
        _logger = logger;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not IIdempotentRequest idempotentRequest)
            return await next();

        Guid requestId = idempotentRequest.RequestId;

        if (await _idempotencyService.IsAlreadyProcessedAsync(requestId))
        {
            _logger.LogWarning("Comando duplicado detectado: {RequestId}", requestId);
            return Activator.CreateInstance<TResponse>()!;
        }

        TResponse? response = await next();

        await _idempotencyService.MarkAsProcessedAsync(requestId);

        return response;
    }
}

