using Microsoft.EntityFrameworkCore;
using Sass.Modules.Events.Api.Events;

namespace Sass.Modules.Events.Api.Database;

public sealed class EventsDbContext : DbContext
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.EVENTS);
    }
}
