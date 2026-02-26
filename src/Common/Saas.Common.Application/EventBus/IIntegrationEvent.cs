namespace Saas.Common.Application.EventBus;

public interface IIntegrationEvent
{
    Guid Id { get; }
    DateTime CreationDate { get; }
}
