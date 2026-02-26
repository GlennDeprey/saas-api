using MediatR;
using Saas.Common.Application.Exceptions;
using Saas.Common.Application.Messaging;
using Saas.Modules.Ticketing.PublicApi;
using Saas.Modules.Users.Application.Users.GetUser;
using Saas.Modules.Users.Domain.Users;

namespace Saas.Modules.Users.Application.Users.RegisterUser;

internal sealed class RegisterUserDomainEventHandler: IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly ITicketingApi _ticketingApi;
    private readonly ISender _sender;
    public RegisterUserDomainEventHandler(ISender sender, ITicketingApi ticketingApi)
    {
        _ticketingApi = ticketingApi;
        _sender = sender;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserQuery(notification.UserId), cancellationToken);
        if (result.IsFailure)
        {
            throw new SaasException(nameof(GetUserQuery), result.Error);
        }

        await _ticketingApi.CreateCustomerAsync(
            result.Value.Id,
            result.Value.Email,
            result.Value.FirstName,
            result.Value.LastName,
            cancellationToken);
    }
}
