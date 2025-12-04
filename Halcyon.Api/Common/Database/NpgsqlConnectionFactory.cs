using System.Data;
using Npgsql;

namespace Halcyon.Api.Common.Database;

public class NpgsqlConnectionFactory(string? connectionString) : IDbConnectionFactory
{
    private readonly string _connectionString =
        connectionString
        ?? throw new InvalidOperationException("Connection string 'Database' is not configured.");

    public IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }
}
