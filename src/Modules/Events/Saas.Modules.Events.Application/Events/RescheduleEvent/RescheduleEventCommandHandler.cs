using Saas.Modules.Events.Application.Abstractions.Clock;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Application.Abstractions.Messaging;
using Saas.Modules.Events.Domain.Abstractions;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Application.Events.RescheduleEvent;

internal sealed class RescheduleEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RescheduleEventCommand>
{
    public async Task<Result> Handle(RescheduleEventCommand request, CancellationToken cancellationToken)
    {
        var currentEvent = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (currentEvent is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure(EventErrors.StartDateInPast);
        }

        currentEvent.Reschedule(request.StartsAtUtc, request.EndsAtUtc);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
