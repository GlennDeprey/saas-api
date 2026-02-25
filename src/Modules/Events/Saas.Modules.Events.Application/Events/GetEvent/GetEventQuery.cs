using Saas.Common.Application.Messaging;

namespace Saas.Modules.Events.Application.Events.GetEvent;

public sealed record GetEventQuery(Guid EventId) : IQuery<EventResponse>;
