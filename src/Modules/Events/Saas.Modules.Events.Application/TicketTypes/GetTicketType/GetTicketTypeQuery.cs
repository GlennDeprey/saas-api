using Saas.Modules.Events.Application.Abstractions.Messaging;

namespace Saas.Modules.Events.Application.TicketTypes.GetTicketType;

public sealed record GetTicketTypeQuery(Guid TicketTypeId) : IQuery<TicketTypeResponse>;
