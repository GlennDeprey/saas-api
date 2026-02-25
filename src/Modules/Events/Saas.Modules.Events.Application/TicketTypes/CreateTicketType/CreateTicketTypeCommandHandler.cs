using Saas.Common.Application.Messaging;
using Saas.Modules.Events.Application.Abstractions.Data;
using Saas.Common.Domain;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Domain.TicketTypes;

namespace Saas.Modules.Events.Application.TicketTypes.CreateTicketType;

internal sealed class CreateTicketTypeCommandHandler(
    IEventRepository eventRepository,
    ITicketTypeRepository ticketTypeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateTicketTypeCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var currentEvent = await eventRepository.GetAsync(request.EventId, cancellationToken);

        if (currentEvent is null)
        {
            return Result.Failure<Guid>(EventErrors.NotFound(request.EventId));
        }

        var ticketType = TicketType.Create(currentEvent, request.Name, request.Price, request.Currency, request.Quantity);

        ticketTypeRepository.Insert(ticketType);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ticketType.Id;
    }
}
