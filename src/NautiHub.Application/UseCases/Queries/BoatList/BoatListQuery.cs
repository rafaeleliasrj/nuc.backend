using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using NautiHub.Application.UseCases.Models.Requests;
using NautiHub.Application.UseCases.Models.Requests.Validators;
using NautiHub.Application.UseCases.Models.Responses;
using NautiHub.Core.Messages.Models;
using NautiHub.Core.Messages.Queries;
using NautiHub.Core.Resources;

namespace NautiHub.Application.UseCases.Queries.BoatList;

public class BoatListQuery
    : Query<QueryResponse<ListPaginationResponse<BoatListResponse>>>
{
    public BoatListFilterRequest Filter { get; set; }
}

public class BoatListFeatureValidator : AbstractValidator<BoatListQuery>
{
    public BoatListFeatureValidator(IServiceProvider serviceProvider)
    {
        var messagesService = serviceProvider.GetRequiredService<MessagesService>();
        RuleFor(r => r.Filter).SetValidator(new BoatListFilterRequestValidator(serviceProvider));
    }
}
