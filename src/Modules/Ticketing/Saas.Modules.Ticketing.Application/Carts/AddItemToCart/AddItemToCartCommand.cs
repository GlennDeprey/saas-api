using Saas.Common.Application.Messaging;

namespace Saas.Modules.Ticketing.Application.Carts.AddItemToCart;

public sealed record AddItemToCartCommand(Guid CustomerId, Guid TicketTypeId, decimal Quantity): ICommand;
