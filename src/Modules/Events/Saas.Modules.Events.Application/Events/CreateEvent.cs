using MediatR;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Modules.Events.Domain.Events;

namespace Saas.Modules.Events.Application.Events;

public sealed record CreateEventCommand(
    string Title, 
    string Description,
    string Location,
    DateTime StartAtUtc,
    DateTime? EndAtUtc): IRequest<Guid>;

internal sealed class CreateEventCommandHandler : IRequestHandler<CreateEventCommand, Guid>
{
    private readonly IEventRepository _eventRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateEventCommandHandler(IEventRepository eventRepository, IUnitOfWork unitOfWork)
    {
        _eventRepository = eventRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var newEvent = new Event
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Location = request.Location,
            StartAtUtc = request.StartAtUtc,
            EndAtUtc = request.EndAtUtc,
            Status = EventStatus.Draft
        };

        _eventRepository.Insert(newEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return newEvent.Id;
    }
}
