using MassTransit;
using Saas.Common.Application.EventBus;

namespace Saas.Common.Infrastructure.EventBus;

internal sealed class EventBus: IEventBus
{
    private readonly IBus _eventBus;
    public EventBus(IBus bus)
    {
        _eventBus = bus;
    }

    public async Task PublishAsync<T>(T integrationEvent, CancellationToken cancellationToken = default)
        where T : IIntegrationEvent
    {
        await _eventBus.Publish(integrationEvent, cancellationToken);
    }
}
