namespace Saas.Modules.Users.PublicApi;

public interface IUsersApi
{
    Task<UserResponse?> GetAsync(Guid userId, CancellationToken cancellationToken = default);
}
