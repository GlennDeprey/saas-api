using Microsoft.EntityFrameworkCore;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Domain.Categories;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Domain.TicketTypes;
using Saas.Modules.Events.Infrastructure.Events;
using Saas.Modules.Events.Infrastructure.TicketTypes;

namespace Saas.Modules.Events.Infrastructure.Database;

public sealed class EventsDbContext : DbContext, IUnitOfWork
{
    public EventsDbContext(DbContextOptions<EventsDbContext> options) : base(options)
    {
    }

    internal DbSet<Event> Events { get; set; }

    internal DbSet<Category> Categories { get; set; }

    internal DbSet<TicketType> TicketTypes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.EVENTS);

        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new TicketTypeConfiguration());
    }
}
