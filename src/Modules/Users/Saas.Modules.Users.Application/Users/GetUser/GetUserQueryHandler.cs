using Dapper;
using Saas.Common.Application.Data;
using Saas.Common.Application.Messaging;
using Saas.Common.Domain;
using Saas.Modules.Users.Domain.Users;

namespace Saas.Modules.Users.Application.Users.GetUser;

internal sealed class GetUserQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        await using var connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT
                 id AS {nameof(UserResponse.Id)},
                 email AS {nameof(UserResponse.Email)},
                 first_name AS {nameof(UserResponse.FirstName)},
                 last_name AS {nameof(UserResponse.LastName)}
             FROM users.users
             WHERE id = @UserId
             """;

        var user = await connection.QuerySingleOrDefaultAsync<UserResponse>(sql, request);

        return user is null ?
            Result.Failure<UserResponse>(UserErrors.NotFound(request.UserId)) :
            (Result<UserResponse>)user;
    }
}
