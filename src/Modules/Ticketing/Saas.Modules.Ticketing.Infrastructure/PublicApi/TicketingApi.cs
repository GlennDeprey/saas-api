using MediatR;
using Saas.Modules.Ticketing.Application.Customers.CreateCustomer;
using Saas.Modules.Ticketing.PublicApi;

namespace Saas.Modules.Ticketing.Infrastructure.PublicApi;

internal sealed class TicketingApi: ITicketingApi
{
    private readonly ISender _sender;
    public TicketingApi(ISender sender)
    {
        _sender = sender;
    }

    public async Task CreateCustomerAsync(Guid customerId, string email, string firstName, string lastName, CancellationToken cancellationToken = default)
    {
        await _sender.Send(new CreateCustomerCommand(customerId, email, firstName, lastName), cancellationToken);
    }
}
