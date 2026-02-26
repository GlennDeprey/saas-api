using Saas.Common.Application.Messaging;

namespace Saas.Modules.Ticketing.Application.Carts.ClearCart;

public sealed record ClearCartCommand(Guid CustomerId) : ICommand;
