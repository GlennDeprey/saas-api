using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Saas.Common.Infrastructure.Database;

public abstract class DesignTimeDbContextFactoryBase<TContext>: IDesignTimeDbContextFactory<TContext>
    where TContext : DbContext
{
    private const string DEFAULT_CONNECTION_STRING = "Host=localhost;Database=saasdb;Username=postgres;Password=postgres";

    protected abstract string Schema { get; }

    protected abstract TContext CreateNewInstance(DbContextOptions<TContext> options);

    public TContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TContext>()
            .UseNpgsql(
                DEFAULT_CONNECTION_STRING,
                npgsqlOptions => npgsqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, Schema))
            .UseSnakeCaseNamingConvention();

        return CreateNewInstance(optionsBuilder.Options);
    }
}
