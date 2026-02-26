using Saas.Modules.Ticketing.Domain.Events;
using Saas.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Saas.Modules.Ticketing.Infrastructure.Events;

internal sealed class EventRepository(TicketingDbContext context): IEventRepository
{
    public async Task<Event?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await context.Events.SingleOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public void Insert(Event newEvent)
    {
        context.Events.Add(newEvent);
    }
}
