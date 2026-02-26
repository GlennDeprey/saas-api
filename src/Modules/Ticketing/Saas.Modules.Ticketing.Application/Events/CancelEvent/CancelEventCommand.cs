using Saas.Common.Application.Messaging;

namespace Saas.Modules.Ticketing.Application.Events.CancelEvent;

public sealed record CancelEventCommand(Guid EventId) : ICommand;
