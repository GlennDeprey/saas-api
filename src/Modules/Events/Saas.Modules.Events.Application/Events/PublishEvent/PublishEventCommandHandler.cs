using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Application.Abstractions.Messaging;
using Saas.Modules.Events.Domain.Abstractions;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Domain.TicketTypes;

namespace Saas.Modules.Events.Application.Events.PublishEvent;

internal sealed class PublishEventCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<PublishEventCommand>
{
    public async Task<Result> Handle(PublishEventCommand request, CancellationToken cancellationToken)
    {
        var currentEvent = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (currentEvent is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        if (!await ticketTypeRepository.ExistsAsync(currentEvent.Id, cancellationToken))
        {
            return Result.Failure(EventErrors.NoTicketsFound);
        }

        var result = currentEvent.Publish();

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
