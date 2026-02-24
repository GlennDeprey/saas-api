using Saas.Modules.Events.Domain.TicketTypes;
using Saas.Modules.Events.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Saas.Modules.Events.Infrastructure.TicketTypes;

internal sealed class TicketTypeRepository : ITicketTypeRepository
{
    private readonly EventsDbContext _context;
    public TicketTypeRepository(EventsDbContext context)
    {
        _context = context;
    }

    public async Task<TicketType?> GetAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TicketTypes.SingleOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<bool> ExistsAsync(Guid eventId, CancellationToken cancellationToken = default)
    {
        return await _context.TicketTypes.AnyAsync(t => t.EventId == eventId, cancellationToken);
    }

    public void Insert(TicketType ticketType)
    {
        _context.TicketTypes.Add(ticketType);
    }
}
