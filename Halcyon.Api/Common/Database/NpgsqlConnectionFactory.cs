using System.Data;
using Npgsql;

namespace Halcyon.Api.Common.Database;

public class NpgsqlConnectionFactory : IDbConnectionFactory
{
    private readonly string _connectionString;

    public NpgsqlConnectionFactory(string? connectionString)
    {
        _connectionString = connectionString
            ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");
    }

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
