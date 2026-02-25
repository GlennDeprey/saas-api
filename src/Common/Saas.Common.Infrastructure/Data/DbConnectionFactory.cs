using Npgsql;
using Saas.Common.Application.Data;
using System.Data.Common;

namespace Saas.Common.Infrastructure.Data;

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
