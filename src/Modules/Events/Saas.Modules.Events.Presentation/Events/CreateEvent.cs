using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Saas.Modules.Events.Application.Events;

namespace Saas.Modules.Events.Presentation.Events;

internal static class CreateEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, ISender sender) =>
        {
            var command = new CreateEventCommand(request.Title,
                request.Description,
                request.Location,
                request.StartAtUtc,
                request.EndAtUtc);

            var eventId = await sender.Send(command);

            return Results.Ok(eventId);
        })
        .WithTags(Tags.EVENTS);
    }

    internal sealed class Request
    {
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required string Location { get; set; }
        public DateTime StartAtUtc { get; set; }
        public DateTime? EndAtUtc { get; set; }
    }
}
