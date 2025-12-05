using Dapper;

namespace Halcyon.Api.Common.Database;

public static class DatabaseExtensions
{
    public static IHostApplicationBuilder AddNpgsqlDapper(
        this IHostApplicationBuilder builder,
        string connectionName
    )
    {
        builder.AddNpgsqlDataSource(connectionName);

        SqlMapper.AddTypeHandler(new DateOnlyHandler());

        return builder;
    }
}
