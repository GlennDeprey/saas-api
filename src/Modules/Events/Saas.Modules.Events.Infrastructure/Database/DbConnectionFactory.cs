using Npgsql;
using Saas.Modules.Events.Application.Abstractions.Data;
using System.Data.Common;

namespace Saas.Modules.Events.Infrastructure.Database;

internal sealed class DbConnectionFactory: IDbConnectionFactory
{
    private readonly NpgsqlDataSource _dataSource;
    public DbConnectionFactory(NpgsqlDataSource dataSource)
    {
        _dataSource = dataSource;
    }
    public async ValueTask<DbConnection> OpenConnectionAsync()
    {
        return await _dataSource.OpenConnectionAsync();
    }
}
