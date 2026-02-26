using Saas.Common.Application.Messaging;
using Saas.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Saas.Modules.Ticketing.Application.Tickets.GetTicketForOrder;

public sealed record GetTicketsForOrderQuery(Guid OrderId) : IQuery<IReadOnlyCollection<TicketResponse>>;
