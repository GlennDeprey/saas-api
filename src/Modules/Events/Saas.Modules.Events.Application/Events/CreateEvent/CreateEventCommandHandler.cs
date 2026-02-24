using Saas.Modules.Events.Application.Abstractions.Clock;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Application.Abstractions.Messaging;
using Saas.Modules.Events.Domain.Abstractions;
using Saas.Modules.Events.Domain.Categories;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Application.Events.CreateEvent;

internal sealed class CreateEventCommandHandler(
    IDateTimeProvider dateTimeProvider,
    ICategoryRepository categoryRepository,
    IEventRepository eventRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateEventCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        if (request.StartsAtUtc < dateTimeProvider.UtcNow)
        {
            return Result.Failure<Guid>(EventErrors.StartDateInPast);
        }

        var category = await categoryRepository.GetAsync(request.CategoryId, cancellationToken);

        if (category is null)
        {
            return Result.Failure<Guid>(CategoryErrors.NotFound(request.CategoryId));
        }

        var result = Event.Create(
            category,
            request.Title,
            request.Description,
            request.Location,
            request.StartsAtUtc,
            request.EndsAtUtc);

        if (result.IsFailure)
        {
            return Result.Failure<Guid>(result.Error);
        }

        eventRepository.Insert(result.Value);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.Id;
    }
}
