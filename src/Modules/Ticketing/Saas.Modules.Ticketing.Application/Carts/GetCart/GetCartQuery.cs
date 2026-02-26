using Saas.Common.Application.Messaging;

namespace Saas.Modules.Ticketing.Application.Carts.GetCart;

public sealed record GetCartQuery(Guid CustomerId) : IQuery<Cart>;
