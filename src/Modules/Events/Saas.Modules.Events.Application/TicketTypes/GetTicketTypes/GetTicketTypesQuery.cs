using Saas.Common.Application.Messaging;
using Saas.Modules.Events.Application.TicketTypes.GetTicketType;

namespace Saas.Modules.Events.Application.TicketTypes.GetTicketTypes;

public sealed record GetTicketTypesQuery(Guid EventId) : IQuery<IReadOnlyCollection<TicketTypeResponse>>;
