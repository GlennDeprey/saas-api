using Saas.Modules.Events.Application.Abstractions.Messaging;

namespace Saas.Modules.Events.Application.TicketTypes.UpdateTicketTypePrice;

public sealed record UpdateTicketTypePriceCommand(Guid TicketTypeId, decimal Price) : ICommand;
