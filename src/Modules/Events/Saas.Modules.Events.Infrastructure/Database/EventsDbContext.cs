using Microsoft.EntityFrameworkCore;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext : DbContext, IUnitOfWork
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
