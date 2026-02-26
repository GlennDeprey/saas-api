using Saas.Modules.Users.Application.Abstractions.Data;
using Saas.Modules.Users.Domain.Users;
using Saas.Modules.Users.Infrastructure.Users;
using Microsoft.EntityFrameworkCore;

namespace Saas.Modules.Users.Infrastructure.Database;

public sealed class UsersDbContext(DbContextOptions<UsersDbContext> options) : DbContext(options), IUnitOfWork
{
    internal DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schemas.USERS);

        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }
}
