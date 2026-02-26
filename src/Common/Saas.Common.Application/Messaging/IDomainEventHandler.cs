using Saas.Common.Domain;
using MediatR;

namespace Saas.Common.Application.Messaging;

public interface IDomainEventHandler<in TDomainEvent>: INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent;
