using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Events.SearchEvents;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Events;

internal class SearchEvents: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/search", async(
            ISender sender,
            Guid ? categoryId,
            DateTime? startDate,
            DateTime? endDate,
            int page = 0,
            int pageSize = 15) =>
        {
            var result = await sender.Send(
                new SearchEventsQuery(categoryId, startDate, endDate, page, pageSize));

            return result.Match(Results.Ok, ApiResults.Problem);
        })
        .WithTags(Tags.EVENTS);
    }
}
