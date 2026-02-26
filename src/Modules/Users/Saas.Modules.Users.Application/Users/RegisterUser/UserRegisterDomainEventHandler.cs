using MediatR;
using Saas.Common.Application.EventBus;
using Saas.Common.Application.Exceptions;
using Saas.Common.Application.Messaging;
using Saas.Modules.Users.Application.Users.GetUser;
using Saas.Modules.Users.Domain.Users;
using Saas.Modules.Users.IntegrationEvents;

namespace Saas.Modules.Users.Application.Users.RegisterUser;

internal sealed class UserRegisterDomainEventHandler: IDomainEventHandler<UserRegisteredDomainEvent>
{
    private readonly IEventBus _eventBus;
    private readonly ISender _sender;
    public UserRegisterDomainEventHandler(ISender sender, IEventBus eventBus)
    {
        _eventBus = eventBus;
        _sender = sender;
    }

    public async Task Handle(UserRegisteredDomainEvent notification, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetUserQuery(notification.UserId), cancellationToken);
        if (result.IsFailure)
        {
            throw new SaasException(nameof(GetUserQuery), result.Error);
        }

        await _eventBus.PublishAsync(
            new UserRegisteredIntegrationEvent(
                notification.Id,
                notification.OccurredOnUtc,
                result.Value.Id,
                result.Value.Email,
                result.Value.FirstName,
                result.Value.LastName),
            cancellationToken);
    }
}
