using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Infrastructure.Database;

namespace Saas.Modules.Events.Infrastructure.Events;

internal sealed class EventRepository : IEventRepository
{
    private readonly EventsDbContext _context;
    public EventRepository(EventsDbContext context)
    {
        _context = context;
    }

    public void Insert(Event newEvent)
    {
        _context.Events.Add(newEvent);
    }
}
