using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Ticketing.Domain.Customers;

namespace Saas.Modules.Ticketing.Application.Carts.ClearCart;

internal sealed class ClearCartCommandHandler(ICustomerRepository customerRepository, CartService cartService)
    : ICommandHandler<ClearCartCommand>
{
    public async Task<Result> Handle(ClearCartCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetAsync(request.CustomerId, cancellationToken);

        if (customer is null)
        {
            return Result.Failure(CustomerErrors.NotFound(request.CustomerId));
        }

        await cartService.ClearAsync(customer.Id, cancellationToken);

        return Result.Success();
    }
}
