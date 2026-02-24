using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Sass.Modules.Events.Api.Database;

namespace Sass.Modules.Events.Api.Events;

public static class CreateEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost("events", async (Request request, EventsDbContext context) =>
        {
            var newEvent = new Event
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                StartAtUtc = request.StartAtUtc,
                EndAtUtc = request.EndAtUtc,
                Status = EventStatus.Draft
            };

            context.Events.Add(newEvent);
            await context.SaveChangesAsync();

            return Results.Created($"/events/{newEvent.Id}", newEvent);
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
