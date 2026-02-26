using MediatR;
using Saas.Modules.Users.Application.Users.GetUser;
using Saas.Modules.Users.PublicApi;

namespace Saas.Modules.Users.Infrastructure.PublicApi;

internal class UsersApi: IUsersApi
{
    private readonly ISender _sender;
    public UsersApi(ISender sender)
    {
        _sender = sender;
    }
    public async Task<Modules.Users.PublicApi.UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default)
    {
        var result = await _sender.Send(new GetUserQuery(userId), cancellationToken);

        if (result.IsFailure)
        {
            return null;
        }

        return new Modules.Users.PublicApi.UserResponse(result.Value.Id, result.Value.Email, result.Value.FirstName, result.Value.LastName);
    }
}
