using Saas.Modules.Events.Domain.Abstractions;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Domain.TicketTypes;
#pragma warning disable CS8618
public sealed class TicketType : Entity
{
    private TicketType()
    {
    }

    public Guid Id { get; private set; }

    public Guid EventId { get; private set; }

    public string Name { get; private set; }

    public decimal Price { get; private set; }

    public string Currency { get; private set; }

    public decimal Quantity { get; private set; }

    public static TicketType Create(
        Event currentEvent,
        string name,
        decimal price,
        string currency,
        decimal quantity)
    {
        var ticketType = new TicketType
        {
            Id = Guid.NewGuid(),
            EventId = currentEvent.Id,
            Name = name,
            Price = price,
            Currency = currency,
            Quantity = quantity
        };

        return ticketType;
    }

    public void UpdatePrice(decimal price)
    {
        if (Price == price)
        {
            return;
        }

        Price = price;

        Raise(new TicketTypePriceChangedDomainEvent(Id, Price));
    }
}
