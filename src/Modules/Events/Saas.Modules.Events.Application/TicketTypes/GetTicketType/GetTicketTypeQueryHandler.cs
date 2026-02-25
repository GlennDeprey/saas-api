using Dapper;
using Saas.Common.Application.Data;
using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Events.Domain.TicketTypes;

namespace Saas.Modules.Events.Application.TicketTypes.GetTicketType;

internal sealed class GetTicketTypeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketTypeQuery, TicketTypeResponse>
{
    public async Task<Result<TicketTypeResponse>> Handle(
        GetTicketTypeQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(TicketTypeResponse.Id)},
                 event_id AS {nameof(TicketTypeResponse.EventId)},
                 name AS {nameof(TicketTypeResponse.Name)},
                 price AS {nameof(TicketTypeResponse.Price)},
                 currency AS {nameof(TicketTypeResponse.Currency)},
                 quantity AS {nameof(TicketTypeResponse.Quantity)}
             FROM events.ticket_types
             WHERE id = @TicketTypeId
             """;

        var ticketType =
            await connection.QuerySingleOrDefaultAsync<TicketTypeResponse>(sql, request);

        return ticketType is null ? 
            Result.Failure<TicketTypeResponse>(TicketTypeErrors.NotFound(request.TicketTypeId)) :
            (Result<TicketTypeResponse>)ticketType;
    }
}
