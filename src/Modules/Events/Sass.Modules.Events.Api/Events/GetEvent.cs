using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Sass.Modules.Events.Api.Database;

namespace Sass.Modules.Events.Api.Events;

public static class GetEvent
{
    public static void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("events/{id:guid}", async (Guid id, EventsDbContext context) =>
        {
            var getEvent = await context.Events
            .Where(e => e.Id == id)
            .Select(e => new EventResponse(
                e.Id,
                e.Title,
                e.Description,
                e.Location,
                e.StartAtUtc,
                e.EndAtUtc))
            .SingleOrDefaultAsync();

            return getEvent is null ? Results.NotFound() : Results.Ok(getEvent);
        })
        .WithTags(Tags.EVENTS);
    }
}
