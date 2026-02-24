using MediatR;
using Dapper;
using Saas.Modules.Events.Application.Abstractions.Data;

namespace Saas.Modules.Events.Application.Events;

public sealed record GetEventQuery(Guid EventId) : IRequest<EventResponse?>;

internal sealed class GetEventQueryHandler: IRequestHandler<GetEventQuery, EventResponse?>
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    public GetEventQueryHandler(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    public async Task<EventResponse?> Handle(GetEventQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await _dbConnectionFactory.OpenConnectionAsync();
        const string sql =
            $"""
             SELECT
                 id AS {nameof(EventResponse.Id)},
                 title AS {nameof(EventResponse.Title)},
                 description AS {nameof(EventResponse.Description)},
                 location AS {nameof(EventResponse.Location)},
                 starts_at_utc AS {nameof(EventResponse.StartAtUtc)},
                 ends_at_utc AS {nameof(EventResponse.EndAtUtc)}
             FROM events.events
             WHERE id = @EventId
             """;

        return await connection.QuerySingleOrDefaultAsync<EventResponse?>(sql, request);
    }
}
