using Microsoft.EntityFrameworkCore;
using Saas.Common.Infrastructure.Database;

namespace Saas.Modules.Users.Infrastructure.Database;

internal sealed class UsersDbContextFactory : DesignTimeDbContextFactoryBase<UsersDbContext>
{
    protected override string Schema => Schemas.USERS;

    protected override UsersDbContext CreateNewInstance(DbContextOptions<UsersDbContext> options) =>
        new(options);
}
