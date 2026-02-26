using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Common.Presentation.Endpoints;
using Saas.Modules.Events.Application.Events.CancelEvent;
using Saas.Common.Presentation.ApiResults;

namespace Saas.Modules.Events.Presentation.Events;

internal class CancelEvent: IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete("events/{id}/cancel", async (Guid id, ISender sender) =>
        {
            var result = await sender.Send(new CancelEventCommand(id));

            return result.Match(Results.NoContent, ApiResults.Problem);
        })
        .WithTags(Tags.EVENTS);
    }
}
