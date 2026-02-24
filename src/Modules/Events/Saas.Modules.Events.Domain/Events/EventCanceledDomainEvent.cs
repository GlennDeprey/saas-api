using Saas.Modules.Events.Domain.Abstractions;

namespace Saas.Modules.Events.Domain.Events;

public sealed class EventCanceledDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
