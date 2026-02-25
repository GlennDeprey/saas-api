using Saas.Common.Application.Messaging;

namespace Saas.Modules.Events.Application.Events.PublishEvent;

public sealed record PublishEventCommand(Guid EventId) : ICommand;
