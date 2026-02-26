using Saas.Common.Application.EventBus;

namespace Saas.Modules.Users.IntegrationEvents;

public sealed class UserRegisteredIntegrationEvent: IntegrationEvent
{
    public UserRegisteredIntegrationEvent(
        Guid id,
        DateTime creationDate,
        Guid userId,
        string email,
        string firstName,
        string lastName)
        : base(id, creationDate)
    {
        UserId = userId;
        Email = email;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid UserId { get; init; }
    public string Email { get; init; }
    public string FirstName { get; init; }
    public string LastName { get; init; }
}
