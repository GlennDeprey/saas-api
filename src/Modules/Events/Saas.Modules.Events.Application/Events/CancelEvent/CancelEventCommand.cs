using Saas.Common.Application.Messaging;

namespace Saas.Modules.Events.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
