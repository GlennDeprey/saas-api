using Microsoft.EntityFrameworkCore;
using Saas.Modules.Events.Infrastructure.Database;
using Saas.Modules.Users.Infrastructure.Database;

namespace Saas.Api.Extensions;

internal static class MigrationExtensions
{
    internal static void ApplyMigrations(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        // Modules
        ApplyMigration<EventsDbContext>(scope);
        ApplyMigration<UsersDbContext>(scope);
    }

    private static void ApplyMigration<TDbContext>(IServiceScope scope)
        where TDbContext : DbContext
    {
        using var context = scope.ServiceProvider.GetRequiredService<TDbContext>();
        context.Database.Migrate();
    }
}
