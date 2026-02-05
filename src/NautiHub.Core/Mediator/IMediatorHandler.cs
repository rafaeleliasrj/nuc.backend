using NautiHub.Core.Messages.Commands;
using NautiHub.Core.Messages.Features;
using NautiHub.Core.Messages.Queries;

namespace NautiHub.Core.Mediator;

public interface IMediatorHandler
{
    public Task<CommandResponse<TResponse>> SendCommand<TResponse>(
        Command<CommandResponse<TResponse>> command
    );
    public Task<CommandResponse> SendCommand(Command<CommandResponse> command);
    public Task<QueryResponse<TResponse>> ExecuteQuery<TResponse>(Query<QueryResponse<TResponse>> query);

    public Task<FeatureResponse<TResponse>> ExecuteFeature<TResponse>(
        Feature<FeatureResponse<TResponse>> feature
    );
    public Task<FeatureResponse> ExecuteFeature(Feature<FeatureResponse> feature);
}
