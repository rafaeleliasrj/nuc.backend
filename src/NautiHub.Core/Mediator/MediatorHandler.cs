using MediatR;
using Microsoft.AspNetCore.Http;
using NautiHub.Core.Extensions;
using NautiHub.Core.Idempotency;
using NautiHub.Core.Messages.Commands;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Messages.Queries;

namespace NautiHub.Core.Mediator;

public class MediatorHandler(IMediator mediator, IHttpContextAccessor httpContextAccessor) : IMediatorHandler
{
    private readonly IMediator _mediator = mediator;
    private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

    public async Task<CommandResponse<TResponse>> SendCommand<TResponse>(
        Command<CommandResponse<TResponse>> command
    ) => await _mediator.Send(command);

    public async Task<CommandResponse> SendCommand(Command<CommandResponse> command) => await _mediator.Send(command);

    public async Task<QueryResponse<TResponse>> ExecuteQuery<TResponse>(
        Query<QueryResponse<TResponse>> query
    ) => await _mediator.Send(query);

    public async Task<FeatureResponse<TResponse>> ExecuteFeature<TResponse>(
        Feature<FeatureResponse<TResponse>> feature)
    {
        InjectRequestIdIfApplicable(feature);
        return await _mediator.Send(feature);
    }

    public async Task<FeatureResponse> ExecuteFeature(Feature<FeatureResponse> feature)
    {
        InjectRequestIdIfApplicable(feature);
        return await _mediator.Send(feature);
    }

    private void InjectRequestIdIfApplicable(object request)
    {
        if (request is IIdempotentRequest)
        {
            System.Reflection.PropertyInfo? requestIdProperty = request.GetType().GetProperty(nameof(IIdempotentRequest.RequestId));
            if (requestIdProperty != null && requestIdProperty.CanWrite)
            {
                Guid requestId = _httpContextAccessor.HttpContext?.GetRequestId() ?? Guid.NewGuid();
                requestIdProperty.SetValue(request, requestId);
            }
        }
    }
}
