using Saas.Modules.Events.Domain.Abstractions;
using Saas.Modules.Events.Domain.Categories;

namespace Saas.Modules.Events.Domain.Events;
#pragma warning disable CS8618
public sealed class Event : Entity
{
    private Event()
    {
    }

    public Guid Id { get; private set; }

    public Guid CategoryId { get; private set; }

    public string Title { get; private set; }

    public string Description { get; private set; }

    public string Location { get; private set; }

    public DateTime StartsAtUtc { get; private set; }

    public DateTime? EndsAtUtc { get; private set; }

    public EventStatus Status { get; private set; }

    public static Result<Event> Create(
        Category category,
        string title,
        string description,
        string location,
        DateTime startsAtUtc,
        DateTime? endsAtUtc)
    {
        if (endsAtUtc.HasValue && endsAtUtc < startsAtUtc)
        {
            return Result.Failure<Event>(EventErrors.EndDatePrecedesStartDate);
        }

        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            CategoryId = category.Id,
            Title = title,
            Description = description,
            Location = location,
            StartsAtUtc = startsAtUtc,
            EndsAtUtc = endsAtUtc,
            Status = EventStatus.Draft
        };

        newEvent.Raise(new EventCreatedDomainEvent(newEvent.Id));

        return newEvent;
    }

    public Result Publish()
    {
        if (Status != EventStatus.Draft)
        {
            return Result.Failure(EventErrors.NotDraft);
        }

        Status = EventStatus.Published;

        Raise(new EventPublishedDomainEvent(Id));

        return Result.Success();
    }

    public void Reschedule(DateTime startsAtUtc, DateTime? endsAtUtc)
    {
        if (StartsAtUtc == startsAtUtc && EndsAtUtc == endsAtUtc)
        {
            return;
        }

        StartsAtUtc = startsAtUtc;
        EndsAtUtc = endsAtUtc;

        Raise(new EventRescheduledDomainEvent(Id, StartsAtUtc, EndsAtUtc));
    }

    public Result Cancel(DateTime utcNow)
    {
        if (Status == EventStatus.Canceled)
        {
            return Result.Failure(EventErrors.AlreadyCanceled);
        }

        if (StartsAtUtc < utcNow)
        {
            return Result.Failure(EventErrors.AlreadyStarted);
        }

        Status = EventStatus.Canceled;

        Raise(new EventCanceledDomainEvent(Id));

        return Result.Success();
    }
}
