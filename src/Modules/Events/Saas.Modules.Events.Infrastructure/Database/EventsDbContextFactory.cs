using Microsoft.EntityFrameworkCore;
using Saas.Common.Infrastructure.Database;

namespace Saas.Modules.Events.Infrastructure.Database;

internal sealed class UsersDbContextFactory: DesignTimeDbContextFactoryBase<EventsDbContext>
{
    protected override string Schema => Schemas.EVENTS;

    protected override EventsDbContext CreateNewInstance(DbContextOptions<EventsDbContext> options) =>
        new(options);
}
