using Dapper;
using Saas.Common.Application.Data;
using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Ticketing.Application.Tickets.GetTicket;
using Saas.Modules.Ticketing.Domain.Tickets;

namespace Saas.Modules.Ticketing.Application.Tickets.GetTicketByCode;

internal sealed class GetTicketByCodeQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetTicketByCodeQuery, TicketResponse>
{
    public async Task<Result<TicketResponse>> Handle(GetTicketByCodeQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
            SELECT
                id AS {nameof(TicketResponse.Id)},
                customer_id AS {nameof(TicketResponse.CustomerId)},
                order_id AS {nameof(TicketResponse.OrderId)},
                event_id AS {nameof(TicketResponse.EventId)},
                ticket_type_id AS {nameof(TicketResponse.TicketTypeId)},
                code AS {nameof(TicketResponse.Code)},
                created_at_utc AS {nameof(TicketResponse.CreatedAtUtc)}
            FROM ticketing.tickets
            WHERE code = @Code
            """;

        var ticket = await connection.QuerySingleOrDefaultAsync<TicketResponse>(sql, request);

        return ticket is null ?
            Result.Failure<TicketResponse>(TicketErrors.NotFound(request.Code)) :
            (Result<TicketResponse>)ticket;
    }
}
