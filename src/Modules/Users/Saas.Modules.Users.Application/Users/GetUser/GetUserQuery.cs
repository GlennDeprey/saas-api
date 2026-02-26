using Saas.Common.Application.Messaging;

namespace Saas.Modules.Users.Application.Users.GetUser;

public sealed record GetUserQuery(Guid UserId): IQuery<UserResponse>;
