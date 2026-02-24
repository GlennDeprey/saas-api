namespace Saas.Modules.Events.Domain.Events;

public interface IEventRepository
{
    void Insert(Event newEvent);
}
