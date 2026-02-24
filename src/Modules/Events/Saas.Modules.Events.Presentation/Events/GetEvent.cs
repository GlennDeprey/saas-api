using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Modules.Events.Application.Events;

namespace Saas.Modules.Events.Presentation.Events;

internal static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, ISender sender) =>
        {
            var getEvent = await sender.Send(new GetEventQuery(id));

            return getEvent is null ? Results.NotFound() : Results.Ok(getEvent);
        })
        .WithTags(Tags.EVENTS);
    }
}
