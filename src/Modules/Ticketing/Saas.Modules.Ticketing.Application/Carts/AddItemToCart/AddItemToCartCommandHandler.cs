using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Events.PublicApi;
using Saas.Modules.Ticketing.Domain.Customers;
using Saas.Modules.Ticketing.Domain.Events;

namespace Saas.Modules.Ticketing.Application.Carts.AddItemToCart;

internal sealed class AddItemToCartCommandHandler: ICommandHandler<AddItemToCartCommand>
{
    private readonly CartService _cartService;
    private readonly ICustomerRepository _customerRepository;
    private readonly IEventsApi _eventsApi;

    public AddItemToCartCommandHandler(
        CartService cartService,
        ICustomerRepository customerRepository,
        IEventsApi eventsApi)
    {
        _cartService = cartService;
        _customerRepository = customerRepository;
        _eventsApi = eventsApi;
    }

    public async Task<Result> Handle(AddItemToCartCommand request, CancellationToken cancellationToken)
    {
        var customer = await _customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        var ticketType = await _eventsApi.GetTicketTypeAsync(request.TicketTypeId, cancellationToken);

        if (ticketType is null)
        {
            return Result.Failure(TicketTypeErrors.NotFound(request.TicketTypeId));
        }

        var cartItem = new CartItem
        {
            TicketTypeId = ticketType.Id,
            Price = ticketType.Price,
            Quantity = request.Quantity,
            Currency = ticketType.Currency
        };

        await _cartService.AddItemAsync(customer.Id, cartItem, cancellationToken);

        return Result.Success();
    }
}
