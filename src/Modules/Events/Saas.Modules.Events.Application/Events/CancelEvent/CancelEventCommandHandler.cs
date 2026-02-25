using Saas.Common.Application.Messaging;
using Saas.Common.Application.Clock;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Common.Domain;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Application.Events.CancelEvent;

internal sealed class CancelEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CancelEventCommand>
{
    public async Task<Result> Handle(CancelEventCommand request, CancellationToken cancellationToken)
    {
        var currentEvent = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (currentEvent is null)
        {
            return Result.Failure(EventErrors.NotFound(request.EventId));
        }

        var result = currentEvent.Cancel(dateTimeProvider.UtcNow);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
