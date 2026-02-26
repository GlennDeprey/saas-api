namespace Saas.Common.Application.EventBus;

public abstract class IntegrationEvent: IIntegrationEvent
{
    public Guid Id { get; init; }
    public DateTime CreationDate { get; init; }
    protected IntegrationEvent(Guid id, DateTime creationDate)
    {
        Id = id;
        CreationDate = creationDate;
    }
}