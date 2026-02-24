using Saas.Modules.Events.Domain.Abstractions;

namespace Saas.Modules.Events.Domain.Events;

public sealed class EventPublishedDomainEvent(Guid eventId) : DomainEvent
{
    public Guid EventId { get; init; } = eventId;
}
