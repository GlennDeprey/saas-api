using MassTransit;
using MediatR;
using Saas.Common.Application.Exceptions;
using Saas.Modules.Ticketing.Application.Customers.CreateCustomer;
using Saas.Modules.Users.IntegrationEvents;

namespace Saas.Modules.Ticketing.Presentation.Customers;

public sealed class UserRegisteredIntegrationEventConsumer: IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ISender _sender;
    public UserRegisteredIntegrationEventConsumer(ISender sender)
    {
        _sender = sender;
    }

    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var result = await _sender.Send(new CreateCustomerCommand(
            context.Message.UserId,
            context.Message.Email,
            context.Message.FirstName,
            context.Message.LastName));

        if (result.IsFailure)
        {
            throw new SaasException(nameof(CreateCustomerCommand), result.Error);
        }
    }
}
