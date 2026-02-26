using MediatR;

namespace Saas.Common.Domain;

public interface IDomainEvent: INotification
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}
