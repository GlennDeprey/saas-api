using Microsoft.EntityFrameworkCore;
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

    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event newEvent)
    {
        _context.Events.Add(newEvent);
    }
}
