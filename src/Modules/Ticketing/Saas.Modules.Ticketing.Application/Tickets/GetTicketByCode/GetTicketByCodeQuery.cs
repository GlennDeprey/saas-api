using Saas.Common.Application.Messaging;
using Saas.Modules.Ticketing.Application.Tickets.GetTicket;

namespace Saas.Modules.Ticketing.Application.Tickets.GetTicketByCode;

public sealed record GetTicketByCodeQuery(string Code) : IQuery<TicketResponse>;
