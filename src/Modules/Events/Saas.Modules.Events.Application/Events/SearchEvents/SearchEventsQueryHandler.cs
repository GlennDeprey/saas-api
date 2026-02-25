using System.Data.Common;
using Dapper;
using Saas.Common.Domain;
using Saas.Modules.Events.Domain.Events;
using Saas.Modules.Events.Application.Events.GetEvents;
using Saas.Common.Application.Messaging;
using Saas.Common.Application.Data;

namespace Saas.Modules.Events.Application.Events.SearchEvents;

internal sealed class SearchEventsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<SearchEventsQuery, SearchEventsResponse>
{

    public async Task<Result<SearchEventsResponse>> Handle(
        SearchEventsQuery request,
        CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        var parameters = new SearchEventsParameters(
            (int)EventStatus.Published,
            request.CategoryId,
            request.StartDate?.Date,
            request.EndDate?.Date,
            request.PageSize,
            (request.Page - 1) * request.PageSize);

        var events = await GetEventsAsync(connection, parameters);

        var totalCount = await CountEventsAsync(connection, parameters);

        return new SearchEventsResponse(request.Page, request.PageSize, totalCount, events);
    }

    private static async Task<IReadOnlyCollection<EventResponse>> GetEventsAsync(
        DbConnection connection,
        SearchEventsParameters parameters)
    {
        const string sql =
            $"""
             SELECT
                 id AS {nameof(EventResponse.Id)},
                 category_id AS {nameof(EventResponse.CategoryId)},
                 title AS {nameof(EventResponse.Title)},
                 description AS {nameof(EventResponse.Description)},
                 location AS {nameof(EventResponse.Location)},
                 starts_at_utc AS {nameof(EventResponse.StartsAtUtc)},
                 ends_at_utc AS {nameof(EventResponse.EndsAtUtc)}
             FROM events.events
             WHERE
                status = @Status AND
                (@CategoryId IS NULL OR category_id = @CategoryId) AND
                (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
                (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
             ORDER BY starts_at_utc
             OFFSET @Skip
             LIMIT @Take
             """;

        return (await connection.QueryAsync<EventResponse>(sql, parameters)).AsList();
    }

    private static async Task<int> CountEventsAsync(DbConnection connection, SearchEventsParameters parameters)
    {
        const string sql =
            """
            SELECT COUNT(*)
            FROM events.events
            WHERE
               status = @Status AND
               (@CategoryId IS NULL OR category_id = @CategoryId) AND
               (@StartDate::timestamp IS NULL OR starts_at_utc >= @StartDate::timestamp) AND
               (@EndDate::timestamp IS NULL OR ends_at_utc >= @EndDate::timestamp)
            """;

        return await connection.ExecuteScalarAsync<int>(sql, parameters);
    }

    private sealed record SearchEventsParameters(
        int Status,
        Guid? CategoryId,
        DateTime? StartDate,
        DateTime? EndDate,
        int Take,
        int Skip);
}
