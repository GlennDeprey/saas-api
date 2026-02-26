using MediatR;
using Saas.Modules.Events.Application.TicketTypes.GetTicketType;
using Saas.Modules.Events.PublicApi;

namespace Saas.Modules.Events.Infrastructure.PublicApi;

internal sealed class EventsApi: IEventsApi
{
    private readonly ISender _sender;
    public EventsApi(ISender sender)
    {
        _sender = sender;
    }

    public async Task<Modules.Events.PublicApi.TicketTypeResponse?> GetTicketTypeAsync(Guid ticketTypeId, CancellationToken cancellationToken)
    {
        var result = await _sender.Send(new GetTicketTypeQuery(ticketTypeId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new Modules.Events.PublicApi.TicketTypeResponse(
            result.Value.Id,
            result.Value.EventId,
            result.Value.Name,
            result.Value.Price,
            result.Value.Currency,
            result.Value.Quantity);
    }
}
