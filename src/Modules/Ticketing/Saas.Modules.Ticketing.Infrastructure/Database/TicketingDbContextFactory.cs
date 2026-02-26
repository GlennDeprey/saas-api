using Microsoft.EntityFrameworkCore;
using Saas.Common.Infrastructure.Database;

namespace Saas.Modules.Ticketing.Infrastructure.Database;

internal sealed class UsersDbContextFactory: DesignTimeDbContextFactoryBase<TicketingDbContext>
{
    protected override string Schema => Schemas.TICKETING;

    protected override TicketingDbContext CreateNewInstance(DbContextOptions<TicketingDbContext> options) =>
        new(options);
}
