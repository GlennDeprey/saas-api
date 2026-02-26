using Saas.Modules.Ticketing.Domain.Events;
using Saas.Modules.Ticketing.Domain.Tickets;
using Saas.Modules.Ticketing.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Saas.Modules.Ticketing.Infrastructure.Tickets;

internal sealed class TicketRepository(TicketingDbContext context): ITicketRepository
{
    public async Task<IEnumerable<Ticket>> GetForEventAsync(
        Event currentEvent,
        CancellationToken cancellationToken = default)
    {
        return await context.Tickets.Where(t => t.EventId == currentEvent.Id)
            .ToListAsync(cancellationToken);
    }

    public void InsertRange(IEnumerable<Ticket> tickets)
    {
        context.Tickets.AddRange(tickets);
    }
}
